// This code has been made by Simon VRANA.
// Please ask by email (simon.vrana.pro@gmail.com) before reusing for commercial purpose.

using UnityEngine;
using UnityEngine.UI;

namespace Helyn.Logger.Example
{
	public class LogSender : MonoBehaviour
	{
		[SerializeField] private Button logButton;
		[SerializeField] private Button warningButton;
		[SerializeField] private Button errorButton;
		[SerializeField] private Button assertButton;
		[SerializeField] private Button exceptionButton;

		void Start()
		{
			logButton.onClick.AddListener(() => Debug.Log("This is an example of \"Log\" level log."));
			warningButton.onClick.AddListener(() => Debug.LogWarning("This is an example of a Warning log."));
			errorButton.onClick.AddListener(() => Debug.LogError("This is an example of an Error log."));
			assertButton.onClick.AddListener(() => Debug.LogAssertion("This is an example of an Assertion failure."));
			exceptionButton.onClick.AddListener(() => Debug.LogException(new System.Exception("This is an example of an Exception.")));
		}

		void OnDestroy()
		{
			logButton.onClick.RemoveAllListeners();
			warningButton.onClick.RemoveAllListeners();
			errorButton.onClick.RemoveAllListeners();
			assertButton.onClick.RemoveAllListeners();
			exceptionButton.onClick.RemoveAllListeners();
		}
	}
}
