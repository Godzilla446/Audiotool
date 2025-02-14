﻿using Microsoft.Win32;
using Audiotool.repository;
using System.Collections.ObjectModel;
using Audiotool.model;

namespace Audiotool.viewmodel;

public class NativeAudio : ViewModelBase
{

    private ObservableCollection<Audio> _audioFiles;

    public ObservableCollection<Audio> AudioFiles
    {
        get { 
            return _audioFiles; 
        }
        set { 
            _audioFiles = value;
            OnPropertyChanged();
        }
    }

    private Audio _selectedAudio;

    public Audio SelectedAudio
    {
        get { return _selectedAudio; }
        set { _selectedAudio = value; }
    }

    private string _soundSetName;

    public string SoundSetName
    {
        get { 
            return _soundSetName; 
        }
        set {
            _soundSetName = value;
            OnPropertyChanged();
        }
    }

    private string _audioBankName;

    public string AudioBankName
    {
        get
        {
            return _audioBankName;
        }
        set
        {
            _audioBankName = value;
            OnPropertyChanged();
        }
    }






    private NativeAudioRepo _repo = new();

    public RelayCommand AddFilesCommand => new(execute => SelectAudioFiles(), canExecute => true);

    public RelayCommand DeleteCommand => new(execute => RemoveAudioFile(), canExecute => SelectedAudio != null);

    public RelayCommand ExportCommand => new(execute => _repo.BuildAWC(SoundSetName, AudioBankName), canExecute => AudioFiles != null && AudioFiles.Count > 0);


    private void RemoveAudioFile() => AudioFiles = _repo.RemoveAudioFile(SelectedAudio.FileName);

    private void SelectAudioFiles()
    {
        OpenFileDialog dialog = new()
        {
            Multiselect = true
        };

        if (dialog.ShowDialog() != true || dialog.FileNames.Length <= 0) return;
        
        
        foreach (string path in dialog.FileNames)
        {
            Task.Run(async () => await _repo.AddAudioFile(path)).GetAwaiter().GetResult();
        }

        AudioFiles = _repo.GetAudioFiles();
    }
 
    
    public NativeAudio()
    {
        SoundSetName = "special_soundset";
        AudioBankName = "custom_sounds";
    }
}