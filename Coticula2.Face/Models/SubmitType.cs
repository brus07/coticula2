
namespace Coticula2.Face.Models
{
    public class SubmitType
    {
        public static int Submit { get { return 1; } }
        public static int Test { get { return 2; } }

        public int SubmitTypeID { get; set; }

        public string Name { get; set; }
    }
}
