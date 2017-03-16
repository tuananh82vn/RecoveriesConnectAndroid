# Getting Started with HorizontalScrollLayout
Displaying the HorizontalScrollLayout is really simple:

Define the data structure to use within the component:
<pre><code class="csharp">
    public class HScrollViewCell1Data
    {
        public int id {get; set;}
        public string name { get; set; }
        public bool shortSelectionTime { get; set; }
    }
</code></pre>

Design your ContentView which will be displayed in the HorizontalScrollLayout component.
<pre><code class="csharp">
  public class HScrollViewCell1 : ContentView
    {

        protected HScrollViewCell1Data bindedData;
        public object BindedData
        {
            get
            {
                return this.bindedData;
            }
            set
            {
                this.bindedData = (HScrollViewCell1Data)value;
            }
        }

        public HScrollViewCell1()
        {

            this.BackgroundColor = Color.Transparent;
            this.Padding = 10;
            Label iconLabel = new Label
            {
                VerticalOptions = LayoutOptions.End,
                HorizontalOptions = LayoutOptions.Center,
                
                FontSize = Device.OnPlatform(
                    Device.GetNamedSize(NamedSize.Micro, typeof(Label)),
                    Device.GetNamedSize(NamedSize.Micro, typeof(Label)),
                    Device.GetNamedSize(NamedSize.Micro, typeof(Label))
                ),
                
                BackgroundColor = Color.Transparent
            };
            iconLabel.SetBinding(Label.TextProperty, "name");

            var viewLayout = new StackLayout()
            {
                Orientation = StackOrientation.Vertical,
                Children = { iconLabel },
                BackgroundColor = Color.Transparent
            };

            Content = viewLayout;

        }

    }
</code></pre>

In your page, add the HorizontalScrollLayout control:
<pre><code class="csharp">
 
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
	// Ading event handler when items are tapped
    this.hscroll1.ItemSelected += hscroll1And2_ItemSelected;
    this.hscroll1.ItemUnSelected += hscroll1And2_ItemUnSelected;	
	
</code></pre>

And manage events:
<pre><code class="csharp">
	async void hscroll1And2_ItemSelected(object sender, HScrollEventArgs e)
	{
		HorizontalScrollLayout layout = (HorizontalScrollLayout)sender;
		HScrollViewCell1Data record = (HScrollViewCell1Data)e.Data;
		if (record.shortSelectionTime)
		{
			await Task.Delay(250);
			layout.UnselectItem(record);
		}

	}
</code></pre>
