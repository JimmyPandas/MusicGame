using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System.IO;
using System;

public class FFmpegExecutableRunner {

	/* This method is ued to run the ffmpeg executable to segment any audio. */
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

	/* This method is ued to run the ffmpeg executable to convert mp3 file
	 * to wav file. 
	*/
	public void ConvertMp3toWav(string audioPath, string outputPath, string searchPath) {
		try {
			Process process = new Process ();
			process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
			process.StartInfo.FileName = searchPath + "/ffmpeg/ffmpeg";

			if(!File.Exists(outputPath)) {
				process.StartInfo.Arguments = "-i " + audioPath + " " +  outputPath;
				process.Start ();
				process.WaitForExit ();
			}
		} catch (System.Exception e) {

		}
	}


}
