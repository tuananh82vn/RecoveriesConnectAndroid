using System;
using System.Threading.Tasks;

using Xamarin.Forms;
using mcpNetwork.Components.Forms;
using HScrollDemo.Views.Xamarin.Forms;

namespace HScrollDemo
{
    public class FormsPage : ContentPage
    {

        private HorizontalScrollLayout hscroll1;
        private HorizontalScrollLayout hscroll2;

        private HorizontalScrollLayout hscroll3;
        private HorizontalScrollLayout hscroll4;
        
        public FormsPage()
        {

            this.Padding = new Thickness(30);

            var dataHScroll1 = new HScrollViewCell1Data[] {
                new HScrollViewCell1Data { id = 1, name = "Item 1", shortSelectionTime = false},
                new HScrollViewCell1Data { id = 2, name = "Item 2", shortSelectionTime = false},
                new HScrollViewCell1Data { id = 3, name = "Item 3", shortSelectionTime = false},
                new HScrollViewCell1Data { id = 4, name = "Item 4", shortSelectionTime = true},
                new HScrollViewCell1Data { id = 5, name = "Item 5", shortSelectionTime = false},
                new HScrollViewCell1Data { id = 6, name = "Item 6", shortSelectionTime = false},
                new HScrollViewCell1Data { id = 7, name = "Item 7", shortSelectionTime = true},
                new HScrollViewCell1Data { id = 8, name = "Item 8", shortSelectionTime = false},
                new HScrollViewCell1Data { id = 9, name = "Item 9", shortSelectionTime = false},
                new HScrollViewCell1Data { id = 10, name = "Item 10", shortSelectionTime = false},
                new HScrollViewCell1Data { id = 11, name = "Item 11", shortSelectionTime = false},
                new HScrollViewCell1Data { id = 12, name = "Item 12", shortSelectionTime = false},
                new HScrollViewCell1Data { id = 13, name = "Item 13", shortSelectionTime = false},
                new HScrollViewCell1Data { id = 14, name = "Item 14", shortSelectionTime = true},
                new HScrollViewCell1Data { id = 15, name = "Item 15", shortSelectionTime = false},
                new HScrollViewCell1Data { id = 16, name = "Item 16", shortSelectionTime = false},
                new HScrollViewCell1Data { id = 17, name = "Item 17", shortSelectionTime = true},
                new HScrollViewCell1Data { id = 18, name = "Item 18", shortSelectionTime = false},
                new HScrollViewCell1Data { id = 19, name = "Item 19", shortSelectionTime = false},
                new HScrollViewCell1Data { id = 20, name = "Item 20", shortSelectionTime = false}
            };
            this.hscroll1 = new HorizontalScrollLayout
            {
                ItemSource = dataHScroll1, 
                ItemTemplate = new DataTemplate(typeof(HScrollViewCell1)),
                HeightRequest = 35,
                SelectionBackGroundColor = Color.Fuchsia,
                MultiSelection = false
            };
            this.hscroll1.ItemSelected += hscroll1And2_ItemSelected;
            this.hscroll1.ItemUnSelected += hscroll1And2_ItemUnSelected;

            Label hScroll1Label = new Label
            {
                Text = "Horizontal scrollbar with unique selection"
            };

            StackLayout slHScroll1 = new StackLayout
            {
                Spacing = 5,
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.Start,
                Children = { this.hscroll1, hScroll1Label }
            };
            
            var dataHScroll2 = new HScrollViewCell2Data[] {
                new HScrollViewCell2Data { id = 1, name = "Item 1", resetSelection = false, icon = "icon1.png"},
                new HScrollViewCell2Data { id = 2, name = "Item 2", resetSelection = false, icon = "icon2.png"},
                new HScrollViewCell2Data { id = 3, name = "Item 3", resetSelection = false, icon = "icon3.png"},
                new HScrollViewCell2Data { id = 4, name = "Item 4", resetSelection = true, icon = "icon4.png"},
                new HScrollViewCell2Data { id = 5, name = "Item 5", resetSelection = false, icon = "icon5.png"},
                new HScrollViewCell2Data { id = 6, name = "Item 6", resetSelection = false, icon = "icon6.png"},
                new HScrollViewCell2Data { id = 7, name = "Item 7", resetSelection = true, icon = "icon7.png"},
                new HScrollViewCell2Data { id = 8, name = "Item 8", resetSelection = false, icon = "icon8.png"},
                new HScrollViewCell2Data { id = 9, name = "Item 9", resetSelection = false, icon = "icon9.png"},
                new HScrollViewCell2Data { id = 10, name = "Item 10", resetSelection = false, icon = "icon0.png"},
                new HScrollViewCell2Data { id = 11, name = "Item 11", resetSelection = false, icon = "icon1.png"},
                new HScrollViewCell2Data { id = 12, name = "Item 12", resetSelection = false, icon = "icon2.png"},
                new HScrollViewCell2Data { id = 13, name = "Item 13", resetSelection = false, icon = "icon3.png"},
                new HScrollViewCell2Data { id = 14, name = "Item 14", resetSelection = true, icon = "icon4.png"},
                new HScrollViewCell2Data { id = 15, name = "Item 15", resetSelection = false, icon = "icon5.png"},
                new HScrollViewCell2Data { id = 16, name = "Item 16", resetSelection = false, icon = "icon6.png"},
                new HScrollViewCell2Data { id = 17, name = "Item 17", resetSelection = true, icon = "icon7.png"},
                new HScrollViewCell2Data { id = 18, name = "Item 18", resetSelection = false, icon = "icon8.png"},
                new HScrollViewCell2Data { id = 19, name = "Item 19", resetSelection = false, icon = "icon9.png"},
                new HScrollViewCell2Data { id = 20, name = "Item 20", resetSelection = false, icon = "icon0.png"}
            };
            this.hscroll2 = new HorizontalScrollLayout
            {
                ItemSource = dataHScroll2,
                ItemTemplate = new DataTemplate(typeof(HScrollViewCell2)),
                HeightRequest = 60,
                SelectionBackGroundColor = Color.Lime,
                MultiSelection = true
            };
            this.hscroll2.ItemSelected += hscroll1And2_ItemSelected;
            this.hscroll2.ItemUnSelected += hscroll1And2_ItemUnSelected;

            Label hScroll2Label = new Label
            {
                Text = "Horizontal scrollbar with multiple selection"
            };

            StackLayout slHScroll2 = new StackLayout
            {
                Spacing = 5,
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.Start,
                Children = { this.hscroll2, hScroll2Label }
            };
            
            hscroll3 = new HorizontalScrollLayout
            {
                ItemSource = dataHScroll2,
                ItemTemplate = new DataTemplate(typeof(HScrollViewCell2)),
                HeightRequest = 60,
                SelectionBackGroundColor = Color.Teal,
                MultiSelection = false
            };
            hscroll3.ItemSelected += hscroll3_ItemSelected;
            hscroll3.ItemUnSelected += hscroll3_ItemUnSelected;

            Label hScroll3Label = new Label
            {
                Text = "Tapped items here will go into the list below"
            };

            StackLayout slHScroll3 = new StackLayout
            {
                Spacing = 5,
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.Start,
                Children = { this.hscroll3, hScroll3Label }
            };


            hscroll4 = new HorizontalScrollLayout
            {
                ItemTemplate = new DataTemplate(typeof(HScrollViewCell2)),
                HeightRequest = 60,
                SelectionBackGroundColor = Color.Teal,
                MultiSelection = false
            };
            hscroll4.ItemSelected += hscroll3_ItemSelected;
            hscroll4.ItemUnSelected += hscroll3_ItemUnSelected;

            Label hScroll4Label = new Label
            {
                Text = "Tapped items here will go into the list above"
            };

            StackLayout slHScroll4 = new StackLayout
            {
                Spacing = 5,
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.Start,
                Children = { this.hscroll4, hScroll4Label }
            };

            Content = new StackLayout
            {
                Spacing = 10,
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.Start,
                Children = { slHScroll1, slHScroll2, slHScroll3, slHScroll4 }
            };
        }


        async void hscroll1And2_ItemSelected(object sender, HScrollEventArgs e)
        {
            HorizontalScrollLayout layout = (HorizontalScrollLayout)sender;
            if (layout.Id.ToString() == this.hscroll1.Id.ToString())
            {
                HScrollViewCell1Data record = (HScrollViewCell1Data)e.Data;
                if (record.shortSelectionTime)
                {
                    await Task.Delay(250);
                    layout.UnselectItem(record);
                }
            }

            if (layout.Id.ToString() == this.hscroll2.Id.ToString())
            {
                HScrollViewCell2Data record = (HScrollViewCell2Data)e.Data;
                if (record.resetSelection)
                {
                    await Task.Delay(250);
                    foreach (object selectedRecord in layout.SelectedItems)
                    {
                        layout.UnselectItem(selectedRecord);
                    }
                }
            }

        }

        void hscroll1And2_ItemUnSelected(object sender, HScrollEventArgs e)
        {
            HorizontalScrollLayout layout = (HorizontalScrollLayout)sender;
            if (layout.Id.ToString() == this.hscroll1.Id.ToString())
            {
                HScrollViewCell1Data record = (HScrollViewCell1Data)e.Data;
            }
            if (layout.Id.ToString() == this.hscroll2.Id.ToString())
            {
                HScrollViewCell2Data record = (HScrollViewCell2Data)e.Data;
            }
        }

        async void hscroll3_ItemSelected(object sender, HScrollEventArgs e)
        {
            HorizontalScrollLayout layout = (HorizontalScrollLayout)sender; 
            HScrollViewCell2Data record = (HScrollViewCell2Data)e.Data;
            await Task.Delay(250);
            layout.RemoveItem(record);
            await Task.Delay(100);
            if (layout.Id.ToString() == this.hscroll3.Id.ToString())
            {
                this.hscroll4.AddItem(record);
                this.hscroll4.SelectItem(record);
            }
            else
            {
                this.hscroll3.AddItem(record);
                this.hscroll3.SelectItem(record);
            }

        }
        
        void hscroll3_ItemUnSelected(object sender, HScrollEventArgs e)
        {
            HScrollViewCell2Data record = (HScrollViewCell2Data)e.Data;
        }

    }
}
