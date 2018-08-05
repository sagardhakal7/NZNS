var tour = new Tour({ backdrop: true,
	onShown: function(tour) {
		var stepElement = getTourElement(tour);
		$(stepElement).after($('.tour-step-background'));
		$(stepElement).after($('.tour-backdrop'));
	},
  steps: [
  {
	element: "#footerrrrrrrr",
    title: "Final SEach lnjkadfgl;kasfdl;ojk",
    content: "Cool Fuckero",
	placement: "bottom",
  },
    {
	element: "#SubinFunction",
    title: "Last checkk",
    content: "Cool Fuckero",
	placement: "top",
  }
]});

tour.init();
tour.start();
tour.restart();

function getTourElement(tour){
    return tour._options.steps[tour._current].element
}