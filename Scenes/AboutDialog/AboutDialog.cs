using Godot;
using System;

public partial class AboutDialog : PanelContainer
{
	[Export] private Button CloseButton;
	[Export] private RichTextLabel ContentLabel;

	public override void _Ready()
	{
		CloseButton.Pressed += OnCloseButtonPressed;
		Visible = false;

		// Set the about content
		SetContent();
	}

	public override void _ExitTree()
	{
		CloseButton.Pressed -= OnCloseButtonPressed;
	}

	private void SetContent()
	{
		if (ContentLabel == null) return;

		ContentLabel.BbcodeEnabled = true;
		ContentLabel.Text = @"[b][font_size=24]Inspector Crawford and the Hallway Murder[/font_size][/b]

[b]Game Jam Entry[/b]
Ludum Dare 58 - Theme: Collector

[b]Developed by:[/b]
Vortiago and Zearan

[b]Asset Attributions:[/b]
• Art:
	- Model and textures by Ulf
	- Nevrax SARL / Winch Gate Properties Ltd. Ryzom. https://atys.wiki.ryzom.com/wiki/Ryzom_Commons:About
	- Ceiling - Batch of 7 Seamless Textures with normalmaps By http://www.benkyoustudio.com
	- 2 tilling wood panel textures By Scribe
	- 6 Panel Door Slab By DataBrace
	- Grandfather Clock By ubunho
	- Objects in a house By TiZiana
	- Book By yd
	- Breakfast set By Fleurman
	- Fabric, Flowers, Seamless Texture With Normalmap By Keith333
	- Royal Dagger By hpalo
	
• Background music:
	Victorian Loop by Joe Baxter-Webb (BossLevelVGM)

[b]Godot Engine:[/b]
This game uses Godot Engine, available under the following license:

Copyright (c) 2014-present Godot Engine contributors.
Copyright (c) 2007-2014 Juan Linietsky, Ariel Manzur.

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the ""Software""), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED ""AS IS"", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.";
	}

	public new void Show()
	{
		Visible = true;
	}

	public new void Hide()
	{
		Visible = false;
	}

	private void OnCloseButtonPressed()
	{
		Hide();
	}
}
