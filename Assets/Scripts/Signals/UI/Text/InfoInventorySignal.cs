namespace Signals
{
    public struct InfoInventorySignal
    {
        private readonly string _nameText;
        private readonly string _infoText;
        
        public string NameText => _nameText;
        public string InfoText => _infoText;

        public InfoInventorySignal(string nameText, string infoText)
        {
            _nameText = nameText;
            _infoText = infoText;
        }
    }
}