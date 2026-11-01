using UnityEngine;
	using UnityEngine.UIElements;

	public class TestPanelRenderer : MonoBehaviour
	{
		#region Inspector Fields

		[SerializeField]
		private PanelRenderer _panel;

		[SerializeField]
		private VisualElementReference<Button> _buttonReference;

		[SerializeField]
		private VisualElementReference<Label> _labelReference;

		private Button _button;
		private Label _label;
		
		#endregion
		
		#region Unity Callbacks
		
		private void Awake()
		{
			Debug.Log("Awake");
			_panel.RegisterUIReloadCallback(OnUIReload);
		}

		private void Start()
		{
			Debug.Log("Start");
		}

		private void OnEnable()
		{
			Debug.Log("On Enable");
		}

		private void OnDestroy()
		{
			_panel.UnregisterUIReloadCallback(OnUIReload);
			_buttonReference.UnregisterReferenceUnloadedCallback(HandleButtonReferenceCallback);
			_labelReference.UnregisterReferenceUnloadedCallback(HandleLabelReferenceCallback);
		}

		#endregion

		#region Methods

		private void OnUIReload(PanelRenderer panel, VisualElement root)
		{
			Debug.Log("UI Reload on frame: " + Time.frameCount);
			_buttonReference.RegisterReferenceResolvedCallback(HandleButtonReferenceCallback);
			_labelReference.RegisterReferenceResolvedCallback(HandleLabelReferenceCallback);
		}

		private void HandleButtonReferenceCallback(Button button)
		{
			Debug.Log("Button Reference Callback");
			_button = button;

			_button.clicked += OnButtonClicked;
		}

		private void HandleLabelReferenceCallback(Label label)
		{
			Debug.Log("Label Reference Callback");
			_label = label;
			_label.text = "Callback!";
		}

		private void OnButtonClicked()
		{
			Debug.Log("Clicked!");
		}

		#endregion
	}