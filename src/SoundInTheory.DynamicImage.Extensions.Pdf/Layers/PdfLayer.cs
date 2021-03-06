﻿using System.IO;
using SoundInTheory.DynamicImage.Util;

namespace SoundInTheory.DynamicImage.Layers
{
	public class PdfLayer : Layer
	{
		public string SourceFileName
		{
			get { return this["SourceFileName"] as string ?? string.Empty; }
			set { this["SourceFileName"] = value; }
		}

		public int PageNumber
		{
			get { return (int)(this["PageNumber"] ?? 1); }
			set { this["PageNumber"] = value; }
		}

		public override bool HasFixedSize
		{
			get { return true; }
		}

		protected override void CreateImage(ImageGenerationContext context)
		{
			GhostscriptUtil.EnsureDll(context);

			string outputFileName = Path.GetTempFileName();

			try
			{
				string filename = FileSourceHelper.ResolveFileName(context, SourceFileName);
				GhostscriptWrapper.GeneratePageThumb(filename, outputFileName, PageNumber, 96, 96);
				Bitmap = new FastBitmap(File.ReadAllBytes(outputFileName));
			}
			finally
			{
				File.Delete(outputFileName);
			}
		}
	}
}
