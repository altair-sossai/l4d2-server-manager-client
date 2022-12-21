using L4D2AntiCheat.DependencyInjection;
using L4D2AntiCheat.Infrastructure.Helpers;
using Microsoft.Extensions.DependencyInjection;

namespace L4D2AntiCheat.Forms;

public partial class OptInForm : Form
{
	public OptInForm()
	{
		InitializeComponent();
	}

	private void NoButton_Click(object sender, EventArgs e)
	{
		Application.Exit();
	}

	private void YesButton_Click(object sender, EventArgs e)
	{
		OptInHelper.Accept();

		using var serviceProvider = ServiceProviderFactory.New();
		var mainForm = serviceProvider.GetRequiredService<MainForm>();
		mainForm.Closed += (_, _) => Application.Exit();

		Hide();

		mainForm.ShowDialog(this);
	}
}