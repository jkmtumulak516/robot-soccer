namespace RobotSoccer.FuzzyLogic
{
    public class FuzzyValue
    {
        public FuzzyValue()
        {
            MetaData = new FuzzyMetaData();
        }

        public void SetMembership(double CrispValue)
        {
            Membership = GetMembership(this, CrispValue);
        }

        public override bool Equals(object obj)
        {
            var newObj = obj as FuzzyValue;

            if (newObj != null)
            {
                return (MetaData.Equals(newObj.MetaData));
            }

            else
            {
                return base.Equals(obj);
            }
        }
        public override int GetHashCode()
        {
            return Index;
        }
        public override string ToString()
        {
            return Name + " >> " + Membership;
        }

        public string Category
        {
            get
            {
                return MetaData.Category;
            }

            set
            {
                MetaData.Category = value;
            }
        }

        public string Name
        {
            get
            {
                return MetaData.Name;
            }

            set
            {
                MetaData.Name = value;
            }
        }

        public int Index
        {
            get
            {
                return MetaData.Index;
            }

            set
            {
                MetaData.Index = value;
            }
        }

        public double Membership { get; internal set; }
        
        public double Center { get; internal set; }
        public double LowerWidth { get; internal set; }
        public double UpperWidth { get; internal set; }
        public int LeftModifier { get; internal set; }
        public int RightModifier { get; internal set; }

        internal GetMembership GetMembership { get; set; }

        public FuzzyMetaData MetaData { get; internal set; }
    }
}
