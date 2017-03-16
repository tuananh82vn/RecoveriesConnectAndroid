using System;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using HScrollDemo.UserControls;

using mcpNetwork.Components.Xaml;

namespace HScrollDemo
{    
    public partial class XamlPage : ContentPage
    {
        public XamlPage()
        {
            InitializeComponent();

            this.LoadHScrolls();
        }
        
        private void LoadHScrolls()
        {
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
            HScroll1.ItemTemplate = new DataTemplate(typeof(HScrollViewCell1));
            HScroll1.ItemSource = dataHScroll1;
            HScroll1.ItemSelected += hscroll1And2_ItemSelected;
            HScroll1.ItemUnSelected += hscroll1And2_ItemUnSelected;

            
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
            HScroll2.ItemTemplate = new DataTemplate(typeof(HScrollViewCell2));
            HScroll2.ItemSource = dataHScroll2;
            HScroll2.ItemSelected += hscroll1And2_ItemSelected;
            HScroll2.ItemUnSelected += hscroll1And2_ItemUnSelected;

            HScroll3.ItemTemplate = new DataTemplate(typeof(HScrollViewCell2));
            HScroll3.ItemSource = dataHScroll2;
            HScroll3.ItemSelected += hscroll3_ItemSelected;
            HScroll3.ItemUnSelected += hscroll3_ItemUnSelected;

            HScroll4.ItemTemplate = new DataTemplate(typeof(HScrollViewCell2));
            HScroll4.ItemSelected += hscroll3_ItemSelected;
            HScroll4.ItemUnSelected += hscroll3_ItemUnSelected;


        }

        async void hscroll1And2_ItemSelected(object sender, HScrollEventArgs e)
        {
            HorizontalScrollLayout layout = (HorizontalScrollLayout)sender;
            ContentView view = e.Template;

            if (layout.Id.ToString() == HScroll1.Id.ToString())
            {
                HScrollViewCell1Data record = (HScrollViewCell1Data)e.Data;
                if (record.shortSelectionTime)
                {
                    await Task.Delay(250);
                    // Unselect item by record
                    layout.UnselectItem(record);
                    // Alternate possibility: unselect the view itself
                    //layout.UnselectItem(view);
                }
            }
            else
            {
                HScrollViewCell2Data record = (HScrollViewCell2Data)e.Data;
                if (record.resetSelection)
                {
                    await Task.Delay(250);
                    foreach (object selectedRecord in layout.SelectedItems)
                    {
                        if (layout.ItemIsSelected(selectedRecord)) {
                            layout.UnselectItem(selectedRecord);
                        }
                    }
                }
            }

        }

        void hscroll1And2_ItemUnSelected(object sender, HScrollEventArgs e)
        {
            HorizontalScrollLayout layout = (HorizontalScrollLayout)sender;
            if (layout.Id.ToString() == HScroll1.Id.ToString())
            {
                HScrollViewCell1Data record = (HScrollViewCell1Data)e.Data;
            }
            if (layout.Id.ToString() == HScroll2.Id.ToString())
            {
                HScrollViewCell2Data record = (HScrollViewCell2Data)e.Data;
            }
        }

        async void hscroll3_ItemSelected(object sender, HScrollEventArgs e)
        {
            HorizontalScrollLayout layout = (HorizontalScrollLayout)sender;
            HScrollViewCell2Data record = (HScrollViewCell2Data)e.Data;
            ContentView view = e.Template;
            await Task.Delay(250);
            layout.RemoveItem(record);
            await Task.Delay(100);
            if (layout.Id.ToString() == HScroll3.Id.ToString())
            {
                HScroll4.AddItem(record);
                HScroll4.SelectItem(record);
            }
            else
            {
                HScroll3.AddItem(record);
                HScroll3.SelectItem(record);
            }

        }

        void hscroll3_ItemUnSelected(object sender, HScrollEventArgs e)
        {
            HScrollViewCell2Data record = (HScrollViewCell2Data)e.Data;
        }


    }
}
