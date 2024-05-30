using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;


namespace DynamicShadows
{
    public class ModEntry : Mod
    {
        private string morningShadow;
        private string afternoonShadow;
        private string eveningShadow;
        private string nightShadow;
        private IModHelper helper;
        private string currentShadow;
        private string prevShadow;

        public override void Entry(IModHelper helper)
        {
            Console.WriteLine("Dynamic Shadows start loading...");
            // Load shadow textures
            morningShadow = "assets/shadow_morning.png";
            afternoonShadow = "assets/shadow_afternoon.png";
            eveningShadow = "assets/shadow_evening.png";
            nightShadow = "assets/shadow_night.png";

            this.currentShadow = morningShadow;
            this.helper = helper;

            // Hook into the game loop
            helper.Events.GameLoop.TimeChanged += OnTimeChanged;
            helper.Events.Content.AssetRequested += this.OnAssetRequested;
            Console.WriteLine("Dynamic Shadows loaded!");
        }

        private void OnTimeChanged(object sender, TimeChangedEventArgs e)
        {
            this.prevShadow = this.currentShadow;
            if (Game1.timeOfDay >= 600 && Game1.timeOfDay < 630)
            {
                this.currentShadow = morningShadow;
            }
            if (Game1.timeOfDay >= 630 && Game1.timeOfDay < 700)
            {
                this.currentShadow = afternoonShadow;
            }
            if (Game1.timeOfDay >= 700 && Game1.timeOfDay < 730)
            {
                this.currentShadow = eveningShadow;
            }
            if (Game1.timeOfDay >= 730)
            {
                this.currentShadow = nightShadow;
            }

            // On state change, invalidate houses texture to reload it
            if (this.prevShadow != this.currentShadow) {
                this.helper.GameContent.InvalidateCache("Buildings/houses");
                Console.WriteLine("Changing state to " + this.currentShadow + " from " + this.prevShadow + " at " + Game1.timeOfDay + " hours.");
            }
            
        }

        private void OnAssetRequested(object sender, AssetRequestedEventArgs e)
        {  
            if (e.Name.IsEquivalentTo("Buildings/houses"))
            {
                e.Edit(asset =>
                {
                    var editor = asset.AsImage();
    
                    IRawTextureData overlay = this.Helper.ModContent.Load<IRawTextureData>(this.currentShadow);
                    editor.PatchImage(overlay);
                });
            }
        }
    }
}