﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MultimediaLibrary
{
    using Interfaces;
    using Repositories;
    using Models;
    using System.Diagnostics;

    /// <summary>
    /// Interaction logic for DisplayPage.xaml
    /// </summary>
    public partial class DisplayArtistPage : Page
    {
        /// <summary>
        /// Constructor that create DisplayArtistPage
        /// </summary>
        /// <remarks>
        /// Display all artists from database
        /// </remarks>
        public DisplayArtistPage()
        {
            InitializeComponent();
            IArtistRepository repo = new ArtistRepository();
            displayArtist.ItemsSource = repo.GetArtists();
        }

        /// <summary>
        /// OpneButton action
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <remarks>
        /// Open artist account on youtube if the path exist
        /// </remarks>
        private void OpenButton_Click(object sender, RoutedEventArgs e)
        {
            Artist artist = displayArtist.SelectedItem as Artist;
            Process myProcess = new Process();
            if (artist.YoutubeAccountPath.Length == 0) MessageBox.Show("No ID");
            else
            {
                try
                {
                    myProcess.StartInfo.UseShellExecute = true;
                    myProcess.StartInfo.FileName = artist.YoutubeAccountPath;
                    myProcess.Start();
                }
                catch (Exception er)
                {
                    MessageBox.Show(er.Message);
                }
            }
        }

        /// <summary>
        /// DisplayButton action
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <remarks>
        /// Open DisplayTrackPage wtih all track of this artist
        /// </remarks>
        private void DisplayButton_Click(object sender, RoutedEventArgs e)
        {
            Artist artist = displayArtist.SelectedItem as Artist;
            this.NavigationService.Navigate(new DisplayTrackPage(artist.ArtistId));
        }

        /// <summary>
        /// UpdateButton action
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <remarks>
        /// Open UpdateTrackPage with indicated artist data
        /// </remarks>
        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            Artist artist = displayArtist.SelectedItem as Artist;
            this.NavigationService.Navigate(new UpdateArtistPage(artist));
        }

        /// <summary>
        /// DeleteButton action
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <remarks>
        /// Delete Artist and all its tracks
        /// </remarks>
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var trackRepo = new TrackRepository();
            var artistRepo = new ArtistRepository();
            Artist artist = displayArtist.SelectedItem as Artist;
            foreach (Track t in artist.Tracks) trackRepo.DeleteTrack(t.TrackId);
            artistRepo.DeleteArtist(artist.ArtistId);
            this.NavigationService.Navigate(new DisplayArtistPage());
        }
    }
}
