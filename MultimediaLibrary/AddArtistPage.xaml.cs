﻿using System;
using System.Collections.Generic;
using System.Text;
//using Syste
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
    using Models;
    using Interfaces;
    using Repositories;
    using System.Threading.Tasks;

    /// <summary>
    /// Interaction logic for AddArtistPage.xaml
    /// </summary>
    public partial class AddArtistPage : Page
    {
        /// <summary>
        /// Constructor of AddArtistPage
        /// </summary>
        /// <remarks>
        /// Open artist form
        /// </remarks>
        public AddArtistPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// saveArtist button action
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <remarks>
        /// Save inscribed data as a new Artist in database
        /// </remarks>
        
        private void saveArtist_Click(object sender, RoutedEventArgs e)
        {
            var resultSearch = new Api();
            Api.SearchApiResult result;

            if (artistNameBox.Text.Length !=0)
            {
                if (ArtistAlreadyExist(artistNameBox.Text)) artistStatusLabel.Content = "Artist already exists";
                else
                {
                    Artist artist = new Artist() { Name = artistNameBox.Text };                   
                    try
                    {
                        result = resultSearch.FindID(artistNameBox.Text, "channel");
                        youtubeAccountPathBox.Text = "https://www.youtube.com/channel/" + result.ID;
                        artist.Subscribers = result.Count;
                    }
                    catch (Exception ex)
                    {                        
                        MessageBox.Show(ex.Message);
                    }
                    if (youtubeAccountPathBox.Text.Length != 0) artist.YoutubeAccountPath = youtubeAccountPathBox.Text;
                    IArtistRepository repo = new ArtistRepository();
                    repo.CreateArtist(artist);
                    artistStatusLabel.Content = "Saved";
                }
            }
            else artistStatusLabel.Content = "Artist name box empty"; 
        }

        /// <summary>
        /// Check if artist exist in database
        /// </summary>
        /// <param name="name"> NAme of artist, requite string argument </param>
        /// <returns> Returns true if artist exist or false if doesn't </returns>
        private bool ArtistAlreadyExist(string name)
        {
            var artistRepo = new ArtistRepository();
            var existingNames = artistRepo.GetArtistNames();

            foreach(string s in existingNames)
            {
                if (s == name) return true;
            }
            return false;
        }
    }
}
