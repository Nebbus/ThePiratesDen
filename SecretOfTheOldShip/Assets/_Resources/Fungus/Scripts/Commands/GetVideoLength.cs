using UnityEngine;
using UnityEngine.UI;

namespace Fungus
{
	[CommandInfo("Video",
		"Get Video Length",
		"Gets the length of a video clip and puts it in a float variable.")]
	public class GetVideoLength : Command 
	{
		public UnityEngine.Video.VideoPlayer videoPlayer;

		[Tooltip("Float variable to store the length of the video clip in.")]
		[VariableProperty(typeof(FloatVariable))]
		[SerializeField] protected FloatVariable variable;

		private UnityEngine.Video.VideoClip videoClip;

		public SetVariable setVariableCommand;
		private SetOperator assign = SetOperator.Assign;

		public override void OnEnter() 
		{
			setVariableCommand = FindObjectOfType<SetVariable> ();
			LengthToVariable ();

			Continue();
		}

		private void LengthToVariable (){
			videoClip = videoPlayer.clip;
			float length = (float)videoClip.length;
			setVariableCommand.SetNewFloatVariable (variable, assign, length);
			setVariableCommand.OnEnter ();
		}
	}
}