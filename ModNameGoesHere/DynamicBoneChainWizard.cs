﻿using System;
using System.Collections.Generic;
using BaseX;
using FrooxEngine;
using FrooxEngine.UIX;

namespace Wizard
{
	public enum UseTagMode
	{
		IgnoreTag,
		IncludeOnlyWithTag,
		ExcludeAllWithTag
	}

	public class DynamicBoneWizard
	{
		public static DynamicBoneWizard _Wizard;
		public static Slot WizardSlot;

		// DynBoneWizard Stuff
		public List<string> listOfFullNames = new List<string>();

		public List<string> listOfPrefixes = new List<string>();
		public List<string> listOfSuffixes = new List<string>();
		public List<string> listOfSingletons = new List<string>();

		public ValueField<UseTagMode> useTagMode;
		public ReferenceField<Slot> ProcessRoot;
		public readonly TextField tag;
		public readonly Text resultsText;
		private int _count;
		private color _buttonColor;
		private LocaleString _text;
		private Slot _scrollAreaRoot;
		private UIBuilder _listBuilder;

		public readonly ValueField<bool> IgnoreInactive;
		public readonly ValueField<bool> IgnoreDisabled;
		public readonly ValueField<bool> IgnoreNonPersistent;
		public readonly ValueField<float> Inertia;
		public readonly ValueField<float> InertiaForce;
		public readonly ValueField<float> Damping;
		public readonly ValueField<float> Elasticity;
		public readonly ValueField<float> Stiffness;
		public readonly ValueField<bool> IsGrabbable;

		public DynamicBoneWizard()
        {
			UniLog.Log("Before Construction");
			_Wizard = this;
			UniLog.Log("Start slots");
			WizardSlot = Engine.Current.WorldManager.FocusedWorld.RootSlot.AddSlot("Dynamic Bone Wizard");
			WizardSlot.GetComponentInChildrenOrParents<Canvas>()?.MarkDeveloper();
			WizardSlot.PersistentSelf = false;
			UniLog.Log("End slots");

			UniLog.Log("Start Data");
			Slot Data = WizardSlot.AddSlot("Data");
			IgnoreInactive = Data.AddSlot("IgnoreInactive").AttachComponent<ValueField<bool>>();
			IgnoreInactive.Value.Value = true;
			IgnoreDisabled = Data.AddSlot("IgnoreDisabled").AttachComponent<ValueField<bool>>();
			IgnoreDisabled.Value.Value = true;
			IgnoreNonPersistent = Data.AddSlot("IgnoreNonPersistent").AttachComponent<ValueField<bool>>();
			IgnoreNonPersistent.Value.Value = true;
			Inertia = Data.AddSlot("Inertia").AttachComponent<ValueField<float>>();
			Inertia.Value.Value = 0.2f;
			InertiaForce = Data.AddSlot("InertiaForce").AttachComponent<ValueField<float>>();
			Inertia.Value.Value = 2.0f;
			Damping = Data.AddSlot("Damping").AttachComponent<ValueField<float>>();
			Inertia.Value.Value = 5.0f;
			Elasticity = Data.AddSlot("Elasticity").AttachComponent<ValueField<float>>();
			Inertia.Value.Value = 100.0f;
			Stiffness = Data.AddSlot("Stiffness").AttachComponent<ValueField<float>>();
			Inertia.Value.Value = 0.2f;
			IsGrabbable = Data.AddSlot("IsGrabbable").AttachComponent<ValueField<bool>>();
			IsGrabbable.Value.Value = false;

			useTagMode = Data.AddSlot("useTagMode").AttachComponent<ValueField<UseTagMode>>();
			useTagMode.Value.Value = UseTagMode.IgnoreTag;
			ProcessRoot = Data.AddSlot("WizardSlot").AttachComponent<ReferenceField<Slot>>();
			ProcessRoot.Reference.Target = null;
			UniLog.Log("End Data");

			UniLog.Log("Start list 1");
			listOfSingletons.AddRange(new string[]
			{
					"<DynBone>",
					"tail",
					"breastupper",
					"leftbooty",
					"rightbooty"
			});
			UniLog.Log("End list 1");

			UniLog.Log("Start list 2");
			listOfPrefixes.AddRange(new string[]
				{
					"breast",
					"breasts",
					"boob",

					"ear",
					"ears",
					"upperear",
					"upper_ear",
					"upper-ear",
					"upper.ear",
					"upper ear",
					"lowerear",
					"lower_ear",
					"lower-ear",
					"lower.ear",
					"lower ear",

					"rear",
					"butt",
					"booty",

					"whisker",
					"whiskers",
					"foreheardwhisker",
					"foreheard_whisker",
					"foreheard-whisker",
					"foreheard.whisker",
					"foreheard whisker",

					"feather",
					"feathers",

					"wing",
					"wings",
					"armwings",
					"arm wings",
					"arm-wings",
					"arm_wings",
					"wristwings",
					"wrist wings",
					"wrist-wings",
					"wrist_wings",

					"tail",

					"fluff",
					"fur",
					"floof",

					"hair",
					"ponytail",
					"pigtail",

					"fronthair",
					"front_hair",
					"front-hair",
					"front.hair",
					"front hair",

					"backhair",
					"back_hair",
					"back-hair",
					"back.hair",
					"back hair",

					"bangs",
					"frontbangs",
					"backbangs",
					"front_bang",
					"back_bang",
					"front_bangs",
					"back_bangs",
					"front-bang",
					"back-bang",
					"front-bangs",
					"back-bangs",
					"front.bang",
					"back.bang",
					"front.bangs",
					"back.bangs",
					"front bang",
					"back bang",
					"front bangs",
					"back bangs",
					"sidebang",
					"sidebangs",
					"side_bang",
					"side_bangs",
					"side-bang",
					"side-bangs",
					"side.bang",
					"side.bangs",
					"side bang",
					"side bangs",

					"cheek",
					"shirt",
					"jacket",
					"lace",
					"skirt",
				});
			UniLog.Log("End list 2");

			UniLog.Log("Start list 3");
			listOfSuffixes.AddRange(new string[] {
					"rt",
					"_rt",
					".rt",
					"-rt",
					" rt",

					"root",
					"_root",
					".root",
					"-root",
					" root",

					"l",
					".l",
					"_l",
					"-l",
					" l",

					"r",
					".r",
					"_r",
					"-r",
					" r",

					"1",
					".1",
					"_1",
					"-1",
					" 1",

					"a",
					".a",
					"_a",
					"-a",
					" a",

					"a1",
					".a1",
					"_a1",
					"-a1",
					" a1",

					"1.r",
					"1.l",

					"01",
					" 01",
					"01.r",
					"01.l",
					"01_r",
					"01_l",
					"01-r",
					"01-l",
					"01 r",
					"01 l",

					"001",
					" 001",
					"001.r",
					"001.l",
					"001_r",
					"001_l",
					"001-r",
					"001-l",
					"001 r",
					"001 l",
				});
			UniLog.Log("End list 3");

			UniLog.Log("Start generate lists");
			GenerateFullNamesList();
			UniLog.Log("End generate lists");

			UniLog.Log("Start UI");
			// Create the UI for the wizard
			WizardSlot.Name = "DynamicBoneChain Management Wizard";
			WizardSlot.Tag = "Developer";
			NeosCanvasPanel neosCanvasPanel = WizardSlot.AttachComponent<NeosCanvasPanel>();
			neosCanvasPanel.Panel.AddCloseButton();
			neosCanvasPanel.Panel.AddParentButton();
			neosCanvasPanel.Panel.Title = "DynamicBoneChain Management Wizard";
			neosCanvasPanel.CanvasSize = new float2(800f, 900f);
			UIBuilder uIBuilder = new UIBuilder(neosCanvasPanel.Canvas);
			List<RectTransform> rectList = uIBuilder.SplitHorizontally(0.5f, 0.5f);
			UniLog.Log("End UI");

			UniLog.Log("Start Settings");
			// Build left hand side UI - options and buttons.
			UIBuilder uIBuilder2 = new UIBuilder(rectList[0].Slot);
			Slot _layoutRoot = uIBuilder2.VerticalLayout(4f, 0f, new Alignment()).Slot;
			uIBuilder2.FitContent(SizeFit.Disabled, SizeFit.MinSize);
			uIBuilder2.Style.Height = 24f;
			UIBuilder uIBuilder3 = uIBuilder2;
			UniLog.Log("End Settings");

			UniLog.Log("Start Armature");
			// Slot reference to which changes will be applied.
			_text = "Armature slot:";
			uIBuilder3.Text(in _text);
			uIBuilder3.Next("Root");
			uIBuilder3.Current.AttachComponent<RefEditor>().Setup(ProcessRoot.Reference);
			uIBuilder3.Spacer(24f);
			UniLog.Log("End Armature");

			UniLog.Log("Start Settings 2");
			// Basic filtering settings for which DynamicBoneChain are accepted for changes or listing.
			_text = "Exclude Inactive:";
			uIBuilder3.HorizontalElementWithLabel(in _text, 0.9f, () => uIBuilder3.BooleanMemberEditor(IgnoreInactive.Value));
			_text = "Exclude Disabled:";
			uIBuilder3.HorizontalElementWithLabel(in _text, 0.9f, () => uIBuilder3.BooleanMemberEditor(IgnoreDisabled.Value));
			_text = "Exclude Non-persistent:";
			uIBuilder3.HorizontalElementWithLabel(in _text, 0.9f, () => uIBuilder3.BooleanMemberEditor(IgnoreNonPersistent.Value));
			_text = "Tag:";
			tag = uIBuilder3.HorizontalElementWithLabel(in _text, 0.2f, () => uIBuilder3.TextField());
			_text = "Tag handling mode:";
			uIBuilder3.Text(in _text);
			uIBuilder3.EnumMemberEditor(useTagMode.Value);
			uIBuilder3.Spacer(24f);
			UniLog.Log("End Settings 2");

			UniLog.Log("Start Settings 3");
			// Dynamic Bone Settings
			_text = "Elasticity";
			uIBuilder3.HorizontalElementWithLabel(in _text, 0.5f, () => uIBuilder3.PrimitiveMemberEditor(Elasticity.Value));
			_text = "Stiffness";
			uIBuilder3.HorizontalElementWithLabel(in _text, 0.5f, () => uIBuilder3.PrimitiveMemberEditor(Stiffness.Value));
			_text = "Inertia";
			uIBuilder3.HorizontalElementWithLabel(in _text, 0.5f, () => uIBuilder3.PrimitiveMemberEditor(Inertia.Value));
			_text = "InertiaForce";
			uIBuilder3.HorizontalElementWithLabel(in _text, 0.5f, () => uIBuilder3.PrimitiveMemberEditor(InertiaForce.Value));
			_text = "Damping";
			uIBuilder3.HorizontalElementWithLabel(in _text, 0.5f, () => uIBuilder3.PrimitiveMemberEditor(Damping.Value));
			_text = "Grabbable";
			uIBuilder3.HorizontalElementWithLabel(in _text, 0.9f, () => uIBuilder3.BooleanMemberEditor(IsGrabbable.Value));
			uIBuilder3.Spacer(24f);
			UniLog.Log("End Settings 3");

			UniLog.Log("Start Settings 4");
			// Buttons for batch actions.
			_text = "List matching DynamicBoneChains";
			uIBuilder3.Button(in _text).LocalPressed += PopulateList;
			_text = "Remove all DynamicBoneChains";
			uIBuilder3.Button(in _text).LocalPressed += RemoveAllDynamicBoneChains;
			_text = "Attach DynamicBoneChains";
			uIBuilder3.Button(in _text).LocalPressed += AttachDynamicBones;
			uIBuilder3.Spacer(24f);
			resultsText = uIBuilder3.Text(in _text);
			UniLog.Log("End Settings 4");

			UniLog.Log("Start Settings 5");
			// Build right hand side UI - list of found DynamicBoneChain.
			UIBuilder uIBuilder4 = new UIBuilder(rectList[1].Slot);
			uIBuilder4.ScrollArea();
			uIBuilder4.VerticalLayout(10f, 4f);
			_scrollAreaRoot = uIBuilder4.FitContent(SizeFit.Disabled, SizeFit.MinSize).Slot;
			UniLog.Log("End Settings 5");

			UniLog.Log("Start Settings 6");
			// Prepare UIBuilder for addding elements to DynamicBoneChain list.
			_listBuilder = uIBuilder4;
			_listBuilder.Style.MinHeight = 40f;
			UniLog.Log("End Settings 6");

			WizardSlot.PositionInFrontOfUser(float3.Backward, distance: 1f);
			UniLog.Log("After Construction");
		}

		private void AttachDynamicBones(IButton button, ButtonEventData eventData)
		{
			// Check whether DynamicBoneChain should be filtered out.
			// If parent has a dynamic bone chain
			// If the slot's name isn't interesting enough
			// If the user says so (by tag, persistent, active)

			WizardSlot.RunSynchronously(() =>
			{
				if (ProcessRoot != null)
				{
					_count = 0;
					recursion(ProcessRoot.Reference.Target);

					PopulateList();
					ShowResults(_count + " DynamicBoneChains attached!");

				}
				else ShowResults("No target root slot set.");
			});
		}

		private void recursion(Slot slot)
		{
			for (int i = 0; i < slot.ChildrenCount; i++)
			{
				if (NameCheck(slot[i].Name.ToLower())
				&& (slot[i].GetComponent<Wiggler>() == null)
				&& !slot[i].Name.Contains("Wiggler")
				&& !slot[i].Name.Contains("Button")
				&& (!IgnoreInactive.Value || slot[i].IsActive)
				&& (!IgnoreNonPersistent.Value || slot[i].IsPersistent)
				&&
				((useTagMode.Value == UseTagMode.IgnoreTag)
				|| (useTagMode.Value == UseTagMode.IncludeOnlyWithTag && slot[i].Tag == tag.TargetString)
				|| (useTagMode.Value == UseTagMode.ExcludeAllWithTag && slot[i].Tag != tag.TargetString)))
				{
					var boneChain = slot[i].AttachComponent<DynamicBoneChain>();
					boneChain.Elasticity.Value = Elasticity.Value;
					boneChain.Stiffness.Value = Stiffness.Value;
					boneChain.Inertia.Value = Inertia.Value;
					boneChain.InertiaForce.Value = InertiaForce.Value;
					boneChain.Damping.Value = Damping.Value;
					boneChain.IsGrabbable.Value = IsGrabbable.Value;
					boneChain.SetupFromChildren(slot[i]);
					slot[i].Tag = "boneAdded";
					_count++;
				}
				else
				{
					recursion(slot[i]);
				}
			}
		}

		private void GenerateFullNamesList()
		{
			listOfFullNames.Clear();

			foreach (var singleton in listOfSingletons)
			{
				listOfFullNames.Add(singleton);
			}

			foreach (var prefix in listOfPrefixes)
			{
				foreach (var suffix in listOfSuffixes)
				{
					listOfFullNames.Add(prefix + suffix);
				}
			}
		}

		private bool NameCheck(string slotCanidateName)
		{
			foreach (var item in listOfFullNames)
			{
				if (slotCanidateName.StartsWith(item))
				{
					return true;

					/* Dated 
					bool lengthCheck = true;
					for (int len = 0; len < item.Length; len++)
					{
						if (name[len] != item[len])
						{
							lengthCheck = false;
						}
					}
					if (lengthCheck)
					{
						return true;
					}*/
				}
			}
			return false;
		}

		private bool ParentTagCheck(Slot child)
		{
			var boneList = new List<DynamicBoneChain>();
			child.GetComponentsInParents<DynamicBoneChain>(boneList);

			// Implies there are no parents with dynamic bones above this slot
			if (boneList.Count == 0)
			{
				return true;
			}
			else
			{
				return false;
			}

		}

		private void CreateScrollListElement(DynamicBoneChain mc)
		{
			Slot _elementRoot = _listBuilder.Next("Element");
			var _refField = _elementRoot.AttachComponent<ReferenceField<DynamicBoneChain>>();
			_refField.Reference.Target = mc;
			UIBuilder _listBuilder2 = new UIBuilder(_elementRoot);
			_listBuilder2.NestInto(_elementRoot);
			_listBuilder2.VerticalLayout(4f, 4f);
			_listBuilder2.HorizontalLayout(10f);
			// Dated?
			_buttonColor = new color(1f, 1f, 1f);
			_listBuilder2.NestOut();
			_listBuilder2.NestOut();
			_listBuilder2.Current.AttachComponent<RefEditor>().Setup(_refField.Reference);
		}

		private void GetAllDynamicBones(Action<DynamicBoneChain> process)
		{
			if (ProcessRoot != null)
			{
				foreach (DynamicBoneChain componentsInChild in ProcessRoot.Reference.Target.GetComponentsInChildren<DynamicBoneChain>(delegate (DynamicBoneChain mc)
				{
					return (!IgnoreInactive.Value || mc.Slot.IsActive)
					&& (!IgnoreDisabled.Value || mc.Enabled)
					&& (!IgnoreNonPersistent.Value || mc.IsPersistent)
					&& ((useTagMode.Value == UseTagMode.IgnoreTag)
					|| (useTagMode.Value == UseTagMode.IncludeOnlyWithTag && mc.Slot.Tag == tag.TargetString)
					|| (useTagMode.Value == UseTagMode.ExcludeAllWithTag && mc.Slot.Tag != tag.TargetString));
				}))
				{
					process(componentsInChild);
				}
			}
			else ShowResults("No target root slot set.");
		}

		private void PopulateList()
		{
			_scrollAreaRoot.DestroyChildren();
			GetAllDynamicBones(delegate (DynamicBoneChain mc)
			{
				CreateScrollListElement(mc);
			});
		}

		private void PopulateList(IButton button, ButtonEventData eventData)
		{
			_count = 0;
			_scrollAreaRoot.DestroyChildren();
			GetAllDynamicBones(delegate (DynamicBoneChain mc)
			{
				CreateScrollListElement(mc);
				_count++;
			});
			ShowResults(_count + " matching DynamicBoneChain(s) listed.");
		}

		private void RemoveAllDynamicBoneChains(IButton button, ButtonEventData eventData)
		{
			List<Slot> tagList = new List<Slot>();
			ProcessRoot.Reference.Target.GetAllChildren(tagList);

			int _count = 0;
			foreach (var slot in tagList)
			{
				DynamicBoneChain boneChain = slot.GetComponent<DynamicBoneChain>();
				if (boneChain != null)
				{
					boneChain.Enabled = false;
					boneChain.Destroy();
					// Implies exsistence of "boneRemoved"
					slot.Tag = string.Empty;
					_count++;
				}
			}

			PopulateList();
			ShowResults($"{_count} DynamicBoneChain(s) deleted.");
		}

		private void ShowResults(string results)
		{
			resultsText.Content.Value = results;
		}
	}
}
