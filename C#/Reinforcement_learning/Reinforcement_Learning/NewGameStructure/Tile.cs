namespace NewGameStructure
{
    public class Tile
    {
        public string ChineseName;
        public string EnglishName;
        public int owner;
        public int code;

        public Tile(int _code, string _englishName, string _chineseName, int _owner = -1)
        {
            code = _code;
            EnglishName = _englishName;
            ChineseName = _chineseName;
            owner = _owner;
        }

        public void SetCode(int inputCode)
        {
            code = inputCode;
        }

        public int GetPatternDigit()
        {
            return code / 100;
        }

        public int GetNumberDigit()
        {
            return (code / 10) % 10;
        }

        public string GetCode()
        {
            return GetPatternDigit().ToString() + GetNumberDigit().ToString();
        }

        public int GetIntCode()
        {
            return code;
        }
    }
}