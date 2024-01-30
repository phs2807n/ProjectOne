//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.7.0
//     from Assets/Input/PlayerInputActions.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @PlayerInputActions: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerInputActions()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerInputActions"",
    ""maps"": [
        {
            ""name"": ""Player"",
            ""id"": ""430e93b4-7ef4-4aef-96c9-4cf17b831785"",
            ""actions"": [
                {
                    ""name"": ""Run"",
                    ""type"": ""Button"",
                    ""id"": ""f66327de-bf50-40e0-8abe-ad04f795d3de"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Evasion"",
                    ""type"": ""Button"",
                    ""id"": ""8274b026-2179-4a66-9ee1-a702149c1a1f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""LeftAction"",
                    ""type"": ""Button"",
                    ""id"": ""d86db2cc-a933-4994-bc66-f43babfd2bd5"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""RightAction"",
                    ""type"": ""Button"",
                    ""id"": ""f61af30b-dc0a-4b9f-8eec-61afb33f6a69"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Action1"",
                    ""type"": ""Button"",
                    ""id"": ""c5acf2a4-a71e-4409-b678-3d4e73b4b930"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Action2"",
                    ""type"": ""Button"",
                    ""id"": ""f7a12ef7-bc8c-4bc3-9b1f-4e60af0017e4"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Action3"",
                    ""type"": ""Button"",
                    ""id"": ""7fa25d8a-bd2b-4902-8a4d-6c00f83eb61f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Status"",
                    ""type"": ""Button"",
                    ""id"": ""bacc3d9d-eec4-4877-bdc5-1e92036f0138"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""change"",
                    ""type"": ""Button"",
                    ""id"": ""39afcc71-f748-4600-823a-765d40aae466"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Skill"",
                    ""type"": ""Button"",
                    ""id"": ""2b7c6d4c-e250-4665-915f-f50e53950713"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Hp"",
                    ""type"": ""Button"",
                    ""id"": ""828bfd88-f477-401d-8424-dd08d6292e59"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Mp"",
                    ""type"": ""Button"",
                    ""id"": ""c1568af2-0998-49a4-a585-93b3808f6034"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""5b8fb910-789b-4882-ba39-499123d3a3db"",
                    ""path"": ""<Keyboard>/leftShift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyBoardMouse"",
                    ""action"": ""Run"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""05369945-4262-4c6d-8065-273c41f357bf"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyBoardMouse"",
                    ""action"": ""Evasion"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e364b1f0-a91d-4df7-893a-74bfb03beb35"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyBoardMouse"",
                    ""action"": ""LeftAction"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b297a93a-dbd3-40ce-93bd-8b26446a711a"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyBoardMouse"",
                    ""action"": ""RightAction"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""850dac61-cb5e-40c5-a803-78cd7f84b292"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Action1"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a7df455b-011e-474b-99c5-dca457bea038"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Action2"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""42ad84ef-2c7f-4ff9-b21b-de1ffebb947b"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Action3"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3e5c26e2-8bd7-4030-a1b7-942ae18a7f0c"",
                    ""path"": ""<Keyboard>/p"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Status"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6881c47f-58dc-49b5-8c96-ffcb559198a5"",
                    ""path"": ""<Keyboard>/f"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""change"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""aacc9908-c8c5-4408-b7f0-befbf71f3e40"",
                    ""path"": ""<Keyboard>/k"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Skill"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""05f99766-d6bf-4f55-a124-1b69b95b9de4"",
                    ""path"": ""<Keyboard>/1"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Hp"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5ced07ae-a70e-476a-bac3-c22d9f250929"",
                    ""path"": ""<Keyboard>/2"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Mp"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""KeyBoardMouse"",
            ""bindingGroup"": ""KeyBoardMouse"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // Player
        m_Player = asset.FindActionMap("Player", throwIfNotFound: true);
        m_Player_Run = m_Player.FindAction("Run", throwIfNotFound: true);
        m_Player_Evasion = m_Player.FindAction("Evasion", throwIfNotFound: true);
        m_Player_LeftAction = m_Player.FindAction("LeftAction", throwIfNotFound: true);
        m_Player_RightAction = m_Player.FindAction("RightAction", throwIfNotFound: true);
        m_Player_Action1 = m_Player.FindAction("Action1", throwIfNotFound: true);
        m_Player_Action2 = m_Player.FindAction("Action2", throwIfNotFound: true);
        m_Player_Action3 = m_Player.FindAction("Action3", throwIfNotFound: true);
        m_Player_Status = m_Player.FindAction("Status", throwIfNotFound: true);
        m_Player_change = m_Player.FindAction("change", throwIfNotFound: true);
        m_Player_Skill = m_Player.FindAction("Skill", throwIfNotFound: true);
        m_Player_Hp = m_Player.FindAction("Hp", throwIfNotFound: true);
        m_Player_Mp = m_Player.FindAction("Mp", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }

    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // Player
    private readonly InputActionMap m_Player;
    private List<IPlayerActions> m_PlayerActionsCallbackInterfaces = new List<IPlayerActions>();
    private readonly InputAction m_Player_Run;
    private readonly InputAction m_Player_Evasion;
    private readonly InputAction m_Player_LeftAction;
    private readonly InputAction m_Player_RightAction;
    private readonly InputAction m_Player_Action1;
    private readonly InputAction m_Player_Action2;
    private readonly InputAction m_Player_Action3;
    private readonly InputAction m_Player_Status;
    private readonly InputAction m_Player_change;
    private readonly InputAction m_Player_Skill;
    private readonly InputAction m_Player_Hp;
    private readonly InputAction m_Player_Mp;
    public struct PlayerActions
    {
        private @PlayerInputActions m_Wrapper;
        public PlayerActions(@PlayerInputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @Run => m_Wrapper.m_Player_Run;
        public InputAction @Evasion => m_Wrapper.m_Player_Evasion;
        public InputAction @LeftAction => m_Wrapper.m_Player_LeftAction;
        public InputAction @RightAction => m_Wrapper.m_Player_RightAction;
        public InputAction @Action1 => m_Wrapper.m_Player_Action1;
        public InputAction @Action2 => m_Wrapper.m_Player_Action2;
        public InputAction @Action3 => m_Wrapper.m_Player_Action3;
        public InputAction @Status => m_Wrapper.m_Player_Status;
        public InputAction @change => m_Wrapper.m_Player_change;
        public InputAction @Skill => m_Wrapper.m_Player_Skill;
        public InputAction @Hp => m_Wrapper.m_Player_Hp;
        public InputAction @Mp => m_Wrapper.m_Player_Mp;
        public InputActionMap Get() { return m_Wrapper.m_Player; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerActions set) { return set.Get(); }
        public void AddCallbacks(IPlayerActions instance)
        {
            if (instance == null || m_Wrapper.m_PlayerActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_PlayerActionsCallbackInterfaces.Add(instance);
            @Run.started += instance.OnRun;
            @Run.performed += instance.OnRun;
            @Run.canceled += instance.OnRun;
            @Evasion.started += instance.OnEvasion;
            @Evasion.performed += instance.OnEvasion;
            @Evasion.canceled += instance.OnEvasion;
            @LeftAction.started += instance.OnLeftAction;
            @LeftAction.performed += instance.OnLeftAction;
            @LeftAction.canceled += instance.OnLeftAction;
            @RightAction.started += instance.OnRightAction;
            @RightAction.performed += instance.OnRightAction;
            @RightAction.canceled += instance.OnRightAction;
            @Action1.started += instance.OnAction1;
            @Action1.performed += instance.OnAction1;
            @Action1.canceled += instance.OnAction1;
            @Action2.started += instance.OnAction2;
            @Action2.performed += instance.OnAction2;
            @Action2.canceled += instance.OnAction2;
            @Action3.started += instance.OnAction3;
            @Action3.performed += instance.OnAction3;
            @Action3.canceled += instance.OnAction3;
            @Status.started += instance.OnStatus;
            @Status.performed += instance.OnStatus;
            @Status.canceled += instance.OnStatus;
            @change.started += instance.OnChange;
            @change.performed += instance.OnChange;
            @change.canceled += instance.OnChange;
            @Skill.started += instance.OnSkill;
            @Skill.performed += instance.OnSkill;
            @Skill.canceled += instance.OnSkill;
            @Hp.started += instance.OnHp;
            @Hp.performed += instance.OnHp;
            @Hp.canceled += instance.OnHp;
            @Mp.started += instance.OnMp;
            @Mp.performed += instance.OnMp;
            @Mp.canceled += instance.OnMp;
        }

        private void UnregisterCallbacks(IPlayerActions instance)
        {
            @Run.started -= instance.OnRun;
            @Run.performed -= instance.OnRun;
            @Run.canceled -= instance.OnRun;
            @Evasion.started -= instance.OnEvasion;
            @Evasion.performed -= instance.OnEvasion;
            @Evasion.canceled -= instance.OnEvasion;
            @LeftAction.started -= instance.OnLeftAction;
            @LeftAction.performed -= instance.OnLeftAction;
            @LeftAction.canceled -= instance.OnLeftAction;
            @RightAction.started -= instance.OnRightAction;
            @RightAction.performed -= instance.OnRightAction;
            @RightAction.canceled -= instance.OnRightAction;
            @Action1.started -= instance.OnAction1;
            @Action1.performed -= instance.OnAction1;
            @Action1.canceled -= instance.OnAction1;
            @Action2.started -= instance.OnAction2;
            @Action2.performed -= instance.OnAction2;
            @Action2.canceled -= instance.OnAction2;
            @Action3.started -= instance.OnAction3;
            @Action3.performed -= instance.OnAction3;
            @Action3.canceled -= instance.OnAction3;
            @Status.started -= instance.OnStatus;
            @Status.performed -= instance.OnStatus;
            @Status.canceled -= instance.OnStatus;
            @change.started -= instance.OnChange;
            @change.performed -= instance.OnChange;
            @change.canceled -= instance.OnChange;
            @Skill.started -= instance.OnSkill;
            @Skill.performed -= instance.OnSkill;
            @Skill.canceled -= instance.OnSkill;
            @Hp.started -= instance.OnHp;
            @Hp.performed -= instance.OnHp;
            @Hp.canceled -= instance.OnHp;
            @Mp.started -= instance.OnMp;
            @Mp.performed -= instance.OnMp;
            @Mp.canceled -= instance.OnMp;
        }

        public void RemoveCallbacks(IPlayerActions instance)
        {
            if (m_Wrapper.m_PlayerActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IPlayerActions instance)
        {
            foreach (var item in m_Wrapper.m_PlayerActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_PlayerActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public PlayerActions @Player => new PlayerActions(this);
    private int m_KeyBoardMouseSchemeIndex = -1;
    public InputControlScheme KeyBoardMouseScheme
    {
        get
        {
            if (m_KeyBoardMouseSchemeIndex == -1) m_KeyBoardMouseSchemeIndex = asset.FindControlSchemeIndex("KeyBoardMouse");
            return asset.controlSchemes[m_KeyBoardMouseSchemeIndex];
        }
    }
    public interface IPlayerActions
    {
        void OnRun(InputAction.CallbackContext context);
        void OnEvasion(InputAction.CallbackContext context);
        void OnLeftAction(InputAction.CallbackContext context);
        void OnRightAction(InputAction.CallbackContext context);
        void OnAction1(InputAction.CallbackContext context);
        void OnAction2(InputAction.CallbackContext context);
        void OnAction3(InputAction.CallbackContext context);
        void OnStatus(InputAction.CallbackContext context);
        void OnChange(InputAction.CallbackContext context);
        void OnSkill(InputAction.CallbackContext context);
        void OnHp(InputAction.CallbackContext context);
        void OnMp(InputAction.CallbackContext context);
    }
}
