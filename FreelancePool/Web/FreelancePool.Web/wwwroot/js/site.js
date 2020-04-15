// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(window).scroll(function () {
	if ($(".navbar").offset().top > 50) {
		$(".navbar-fixed-top").addClass("top-nav-collapse");
	} else {
		$(".navbar-fixed-top").removeClass("top-nav-collapse");
	}
});

function makeInputVisible() {
	let element = document.getElementById("message");

	if (element.style.display == "none") {
		element.style.display = "block";
	}
	else {
		element.style.display = "none"
    }
};

function makeMostWantedVisible() {
	let element = document.getElementById("most-wanted");

	if (element.style.display == "none") {
		element.style.display = "block";
	}
	else {
		element.style.display = "none"
	}
}


