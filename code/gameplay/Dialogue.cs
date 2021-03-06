using Sandbox;
using System.Collections.Generic;

public struct Dialogue
{

	public Human Speaker;
	public string Text;
	public int Duration;
	public bool Angry;
	public float TextSpeed;

	public Dialogue( Human _speaker, string _text, int _duration, bool _angry, float _textSpeed )
	{

		Speaker = _speaker;
		Text = _text;
		Duration = _duration;
		Angry = _angry;
		TextSpeed = _textSpeed;

	}

}

public partial class Human : AnimatedEntity
{

	/// <summary>
	/// Duration is in milliseconds, textSpeed is how fast each character appears
	/// </summary>
	/// <param name="text"></param>
	/// <param name="duration"></param>
	/// <param name="angry"></param>
	/// <param name="textSpeed"></param>
	public async void StartDialogue( string text, int duration, bool angry = false, float textSpeed = 5 )
	{

		if ( Host.IsServer )
		{

			BroadcastDialogue( this, text, duration, angry, textSpeed );

		}

		Event.Run( "StartDialogue", new Dialogue( this, text, duration, angry, textSpeed ) );
		
		SetAnimParameter( "Talking", true );
		SetAnimParameter( "Angry", angry );

		ComputeStartDialogue();

		await Task.Delay( duration );

		Event.Run( "EndDialogue" );

		SetAnimParameter( "Talking", false );
		SetAnimParameter( "Angry", false );

		ComputeEndDialogue();

	}

	[ClientRpc]
	public void BroadcastDialogue( Human speaker, string text, int duration, bool angry, float textSpeed )
	{

		speaker.StartDialogue( text, duration, angry, textSpeed );

	}

	public virtual void ComputeStartDialogue()
	{

		LookAtPosition = xoxoxo.Game.GameCamera.Position;

	}

	public virtual void ComputeEndDialogue()
	{



	}

}
