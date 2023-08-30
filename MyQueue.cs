
namespace Picachu
{
    internal class MyQueue
    {
        public MyPoint Position { get; set; }
        public int Redirect { get; set; }
        public MyPoint? LastDirection { get; set; }

        public MyQueue(MyPoint position, int redirect, MyPoint? lastDirection)
        {
            this.Position = position;
            this.Redirect = redirect;
            this.LastDirection = lastDirection;
        }


    }
}
