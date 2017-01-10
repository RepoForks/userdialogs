using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Android.App;
using Android.OS;

//using Acr.UserDialogs.Fragments;
//using Acr.UserDialogs.Internals;
//using Android.App;
//using Android.Support.V7.App;
//using AlertDialog = Android.Support.V7.App.AlertDialog;


namespace Acr.UserDialogs.AppCompat
{
    public class AppCompatAlertDialog : DialogFragment, IAlertDialog
    {
        public const string FragmentTag = "acr";

        readonly Activity activity;
        readonly IEditTextBuilder editTextBuilder;
        readonly IList<TextEntry> textEntries;
        readonly IList<DialogAction> actions;

        AlertDialog.Builder builder;
        AlertDialog dialog;


        public AppCompatAlertDialog(Activity activity, IEditTextBuilder editTextBuilder)
        {
            this.activity = activity;
            this.editTextBuilder = editTextBuilder;

            this.textEntries = new List<TextEntry>();
            this.TextEntries = new ReadOnlyCollection<TextEntry>(this.textEntries);

            this.actions = new List<DialogAction>();
            this.Actions = new ReadOnlyCollection<DialogAction>(this.actions);
        }


        public override Dialog OnCreateDialog(Bundle bundle)
        {
            return base.OnCreateDialog(bundle);
        }


        public override void OnDetach()
        {
            base.OnDetach();
        }


        //public override void Dismiss()
        //{
        //     this.Activity.RunOnUiThread(frag.Dismiss);
        //}

        public string Message { get; set; }
        public string Title { get; set; }
        public bool IsCancellable { get; set; }
        public DialogAction Positive { get; set; }
        public DialogAction Neutral { get; set; }
        public DialogAction Negative { get; set; }
        public IReadOnlyList<DialogAction> Actions { get; }
        public IReadOnlyList<TextEntry> TextEntries { get; }


        public IAlertDialog Add(TextEntry instance)
        {
            var editText = this.editTextBuilder.Create(this.activity, instance);
            return this;
        }


        public IAlertDialog Add(DialogAction action)
        {
            throw new NotImplementedException();
        }


        public void Show()
        {
            this.builder = new AlertDialog.Builder(this.activity);
            this.activity.RunOnUiThread(() =>
            {
                //frag.Config = config;
                //this.Show(this.activity.SupportFragmentManager, FragmentTag);
            });
        }

        public Action Dismissed { get; set; }
    }
}


/*
 // ALERT
             //var layout = new LinearLayout(context) {
            //    Orientation = Orientation.Vertical,
            //    OverScrollMode = OverScrollMode.IfContentScrolls
            //};
            //var txt = new TextView(context);

            return new AlertDialog
                .Builder(activity)
                .SetCancelable(false)
                .SetMessage(config.Message)
                .SetTitle(config.Title)
                .SetPositiveButton(config.OkText, (o, e) => config.OnAction?.Invoke())
                .Create();


    // ACTIONSHEET
    var dlg = new AlertDialog.Builder(activity, config.AndroidStyleId ?? 0)
                .SetTitle(config.Title);
            //.SetCustomTitle(new TextView(activity) {
            //    Text = config.Title,
            //    TextSize = 18.0f
            //});

            if (config.ItemIcon != null || config.Options.Any(x => x.ItemIcon != null))
            {
                var adapter = new ActionSheetListAdapter(activity, Android.Resource.Layout.SelectDialogItem,
                    Android.Resource.Id.Text1, config);
                dlg.SetAdapter(adapter, (s, a) => config.Options[a.Which].Action?.Invoke());
            }
            else
            {
                var array = config
                    .Options
                    .Select(x => x.Text)
                    .ToArray();

                dlg.SetItems(array, (s, args) => config.Options[args.Which].Action?.Invoke());
            }

            if (config.Destructive != null)
                dlg.SetNegativeButton(config.Destructive.Text, (s, a) => config.Destructive.Action?.Invoke());

            if (config.Cancel != null)
                dlg.SetNeutralButton(config.Cancel.Text, (s, a) => config.Cancel.Action?.Invoke());

            return dlg.Create();


    // CONFIRM
            return new AlertDialog.Builder(activity, config.AndroidStyleId ?? 0)
                .SetCancelable(false)
                .SetMessage(config.Message)
                .SetTitle(config.Title)
                .SetPositiveButton(config.OkText, (s, a) => config.OnAction(true))
                .SetNegativeButton(config.CancelText, (s, a) => config.OnAction(false))
                .Create();

    // LOGIN
var txtUser = new EditText(activity)
            {
                Hint = config.LoginPlaceholder,
                InputType = InputTypes.TextVariationVisiblePassword,
                Text = config.LoginValue ?? String.Empty
            };
            var txtPass = new EditText(activity)
            {
                Hint = config.PasswordPlaceholder ?? "*"
            };
            PromptBuilder.SetInputType(txtPass, InputType.Password);

            var layout = new LinearLayout(activity)
            {
                Orientation = Orientation.Vertical
            };

            txtUser.SetMaxLines(1);
            txtPass.SetMaxLines(1);

            layout.AddView(txtUser, ViewGroup.LayoutParams.MatchParent);
            layout.AddView(txtPass, ViewGroup.LayoutParams.MatchParent);

            return new AlertDialog.Builder(activity, config.AndroidStyleId ?? 0)
                .SetCancelable(false)
                .SetTitle(config.Title)
                .SetMessage(config.Message)
                .SetView(layout)
                .SetPositiveButton(config.OkText, (s, a) =>
                    config.OnAction(new LoginResult(true, txtUser.Text, txtPass.Text))
                )
                .SetNegativeButton(config.CancelText, (s, a) =>
                    config.OnAction(new LoginResult(false, txtUser.Text, txtPass.Text))
                )
                .Create();

    // PROMPT
var txt = new EditText(activity)
            {
                Id = Int32.MaxValue,
                Hint = config.Placeholder
            };
            if (config.Text != null)
                txt.Text = config.Text;

            if (config.MaxLength != null)
                txt.SetFilters(new [] { new InputFilterLengthFilter(config.MaxLength.Value) });

            SetInputType(txt, config.InputType);

            var builder = new AlertDialog.Builder(activity, config.AndroidStyleId ?? 0)
                .SetCancelable(false)
                .SetMessage(config.Message)
                .SetTitle(config.Title)
                .SetView(txt);

            if (config.Positive != null)
            {
                builder.SetPositiveButton(config.Positive.Text, (s, a) =>
                    config.OnAction(new DialogResult<string>(DialogChoice.Positive, txt.Text.Trim()))
                );
            }

            if (config.Neutral != null)
            {
                builder.SetNeutralButton(config.Neutral.Text, (s, a) =>
                    config.OnAction(new DialogResult<string>(DialogChoice.Neutral, txt.Text.Trim()))
                );
            }

            if (config.Negative != null)
            {
                builder.SetNegativeButton(config.Negative.Text, (s, a) =>
                    config.OnAction(new DialogResult<string>(DialogChoice.Negative, txt.Text.Trim()))
                );
            }
            var dialog = builder.Create();
            this.HookTextChanged(dialog, txt, config.OnTextChanged);

            return dialog;



    protected virtual void HookTextChanged(Dialog dialog, EditText txt, Action<PromptTextChangedArgs> onChange)
        {
            if (onChange == null)
                return;

            var buttonId = (int) Android.Content.DialogButtonType.Positive;
            var promptArgs = new PromptTextChangedArgs { Value = String.Empty };

            dialog.ShowEvent += (sender, args) =>
            {
                onChange(promptArgs);
                ((AlertDialog)dialog).GetButton(buttonId).Enabled = promptArgs.IsValid;
            };
            txt.AfterTextChanged += (sender, args) =>
            {
                promptArgs.IsValid = true;
                promptArgs.Value = txt.Text;
                onChange(promptArgs);
                ((AlertDialog)dialog).GetButton(buttonId).Enabled = promptArgs.IsValid;

                if (!txt.Text.Equals(promptArgs.Value))
                {
                    txt.Text = promptArgs.Value;
                    txt.SetSelection(txt.Text.Length);
                }
            };
        }


        //protected override void OnKeyPress(object sender, DialogKeyEventArgs args)
        //{
        //    base.OnKeyPress(sender, args);
        //    args.Handled = false;

        //    switch (args.KeyCode)
        //    {
        //        case Keycode.Back:
        //            args.Handled = true;
        //            if (this.Config.IsCancellable)
        //                this.SetAction(false);
        //        break;

        //        case Keycode.Enter:
        //            args.Handled = true;
        //            this.SetAction(true);
        //        break;
        //    }
        //}


        //protected virtual void SetAction(bool ok)
        //{
        //    var txt = this.Dialog.FindViewById<TextView>(Int32.MaxValue);
        //    this.Config?.OnAction(new PromptResult(ok, txt.Text.Trim()));
        //    this.Dismiss();
        //}
     */
