using InControl;

namespace Asuna.Input
{
    public class DefaultPlayerActionSet : PlayerActionSet
    {
        public readonly PlayerAction Left;
        public readonly PlayerAction Right;
        public readonly PlayerAction Forward;
        public readonly PlayerAction Backward;
        public readonly PlayerTwoAxisAction Move;
        public readonly PlayerAction Jump;
        public readonly PlayerAction Fire;

        public DefaultPlayerActionSet()
        {
            Fire = CreatePlayerAction( "Fire" );
            Jump = CreatePlayerAction( "Jump" );
            Left = CreatePlayerAction( "Left" );
            Right = CreatePlayerAction( "Right" );
            Forward = CreatePlayerAction( "Forward" );
            Backward = CreatePlayerAction( "Backward" );
            Move = CreateTwoAxisPlayerAction( Left, Right, Forward, Backward);
            
            Jump.AddDefaultBinding(InputControlType.Action1);
            Fire.AddDefaultBinding(InputControlType.Action3);
            
            Left.AddDefaultBinding( InputControlType.LeftStickLeft );
            Right.AddDefaultBinding( InputControlType.LeftStickRight );
            Forward.AddDefaultBinding( InputControlType.LeftStickUp );
            Backward.AddDefaultBinding( InputControlType.LeftStickDown );
            
            Left.AddDefaultBinding( InputControlType.DPadLeft );
            Right.AddDefaultBinding( InputControlType.DPadRight );
            Forward.AddDefaultBinding( InputControlType.DPadUp );
            Backward.AddDefaultBinding( InputControlType.DPadDown );
            
        }
    }
    
    
    
}