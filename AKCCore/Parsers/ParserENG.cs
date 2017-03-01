namespace AKCCore{
    public class ParserENG:MyClippingsParser{
        public string Sample { get; set; }
        public Clipping Result = new Clipping();

        public void Parse(){
            //ParseLine1(line, clipping);
            //Parse(Sample);
        }

        protected override void ParseLine2(string a, Clipping b, FormatType c) {
           
        }

        

    }
}
