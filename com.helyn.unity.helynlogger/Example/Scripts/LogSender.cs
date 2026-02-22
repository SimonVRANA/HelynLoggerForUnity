// This code has been made by Simon VRANA.
// Please ask by email (simon.vrana.pro@gmail.com) before reusing for commercial purpose.

using UnityEngine;
using UnityEngine.UI;
using Debug = Helyn.Logger.DebugProxy;

namespace Helyn.Logger.Example
{
	public class LogSender : MonoBehaviour
	{
		[SerializeField] private Button traceButton;
		[SerializeField] private Button traceWithMethodButton;
		[SerializeField] private Button logButton;
		[SerializeField] private Button warningButton;
		[SerializeField] private Button errorButton;
		[SerializeField] private Button assertButton;
		[SerializeField] private Button exceptionButton;

		private void Start()
		{
			traceButton.onClick.AddListener(() => Debug.LogTrace("This is an example of \"Trace\" level log."));
			string value = "\"Trace\" level log";
			traceWithMethodButton.onClick.AddListener(() => Debug.LogTrace(() => "This" +
																				 " is an example of " +
																				 value + " using method for complex string creation."));
			logButton.onClick.AddListener(() => Debug.Log("This is an example of \"Log\" level log."));
			warningButton.onClick.AddListener(() => Debug.LogWarning("This is an example of a Warning log."));
			errorButton.onClick.AddListener(() => Debug.LogError("This is an example of an Error log."));
			assertButton.onClick.AddListener(() => Debug.LogAssertion("This is an example of an Assertion failure."));
			exceptionButton.onClick.AddListener(() => throw new System.Exception("This is an example of an Exception."));
		}

		private void OnDestroy()
		{
			traceButton.onClick.RemoveAllListeners();
			traceWithMethodButton.onClick.RemoveAllListeners();
			logButton.onClick.RemoveAllListeners();
			warningButton.onClick.RemoveAllListeners();
			errorButton.onClick.RemoveAllListeners();
			assertButton.onClick.RemoveAllListeners();
			exceptionButton.onClick.RemoveAllListeners();
		}
	}
}