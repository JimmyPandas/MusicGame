using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System.IO;
using System;

public class FFmpegExecutableRunner {

	/* This method is ued to run the ffmpeg split audio executable file. */
	public void SplitAudio(string searchPath, Clock start_time, string audioSegmentPath, Clock duration) {
		DataManager dataManager = GameObject.Find ("DataManager").GetComponentInChildren<DataManager> ();
		try {
			Process process = new Process ();
			process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
			process.StartInfo.FileName = searchPath + "/ffmpeg/ffmpeg";

			if(File.Exists(audioSegmentPath)) {
				File.Delete(audioSegmentPath);
			}
			process.StartInfo.Arguments = "-i " + dataManager.path + " -acodec copy -t " 
				+ duration.ToString() + " -ss " + start_time.ToString() + " " + audioSegmentPath;
			process.Start ();
			process.WaitForExit ();
		} catch (System.Exception e) {
			
		}
	}

	public void ConvertMp3toWav(string audioPath, string outputPath, string searchPath) {
		try {
			Process process = new Process ();
			process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
			process.StartInfo.FileName = searchPath + "/ffmpeg/ffmpeg";

			if(File.Exists(outputPath)) {
				File.Delete(outputPath);
			}
			process.StartInfo.Arguments = "-i " + audioPath + " " +  outputPath;
			process.Start ();
			process.WaitForExit ();
		} catch (System.Exception e) {

		}
	}


}
