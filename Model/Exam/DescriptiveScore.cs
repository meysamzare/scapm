namespace SCMR_Api.Model
{

    public class DescriptiveScore
    {
        public DescriptiveScore() { }


        public int Id { get; set; }

        public string Name { get; set; }

        public string EnName { get; set; }

        public double FromNumber { get; set; }
        public double ToNumber { get; set; }



        public double Avg => (FromNumber + ToNumber) / 2;
    }
}