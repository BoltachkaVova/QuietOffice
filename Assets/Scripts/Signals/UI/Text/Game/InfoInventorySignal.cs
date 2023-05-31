namespace Signals
{
    public struct InfoInventorySignal
    {
        private readonly string nameText;
        private readonly string infoText;
        
        public string NameText => nameText;
        public string InfoText => infoText;

        public InfoInventorySignal(string nameText, string infoText)
        {
            this.nameText = nameText;
            this.infoText = infoText;
        }
    }
}