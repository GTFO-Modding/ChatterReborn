namespace ChatterReborn.Utils
{
    public struct MethodTokenName
    {


        public static implicit operator string(MethodTokenName token)
        {
            return token.m_name;
        }


        public static implicit operator uint(MethodTokenName token)
        {
            return token.m_tokenID;
        }


        public static implicit operator MethodTokenName(string name)
        {
            return new MethodTokenName(name);
        }


        public static implicit operator MethodTokenName(uint tokenID)
        {
            return new MethodTokenName(tokenID);
        }



        public MethodTokenName(uint tokenID)
        {
            this = default;
            m_tokenID  = tokenID;
        }

        public MethodTokenName(string name)
        {
            this = default;
            m_name  = name;
        }

        public MethodTokenName(string name, uint tokenID)
        {
            m_tokenID = tokenID;
            m_name  = name;
        }

        public bool HasToken => this.m_tokenID > 0;

        public string m_name;

        public uint m_tokenID;
    }
}
