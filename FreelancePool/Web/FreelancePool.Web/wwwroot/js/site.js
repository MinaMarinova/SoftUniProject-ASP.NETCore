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

//jQuery to collapse the navbar on scroll
$(window).scroll(function () {
	if ($(".navbar").offset().top > 50) {
		$(".navbar-fixed-top").addClass("top-nav-collapse");
	} else {
		$(".navbar-fixed-top").removeClass("top-nav-collapse");
	}
});

//jQuery for page scrolling feature - requires jQuery Easing plugin
$(function () {
	$('.navbar-nav li a').bind('click', function (event) {
		var $anchor = $(this);
		$('html, body').stop().animate({
			scrollTop: $($anchor.attr('href')).offset().top
		}, 1500, 'easeInOutExpo');
		event.preventDefault();
	});
	$('.page-scroll a').bind('click', function (event) {
		var $anchor = $(this);
		$('html, body').stop().animate({
			scrollTop: $($anchor.attr('href')).offset().top
		}, 1500, 'easeInOutExpo');
		event.preventDefault();
	});
});

function ShowAndHideInputField(element) {
	if (element.style.display == "none") {
		element.style.display = "block";
	}
	else {
		element.style.display = "none"
	}
}

function makeInputMessageVisible() {
	let element = document.getElementById("message");

	ShowAndHideInputField(element)
};

function makeMostWantedVisible() {
	let element = document.getElementById("most-wanted");

	ShowAndHideInputField(element);
}

function makeInputCategoryVisible() {
	let element = document.getElementById("delete-category");

	ShowAndHideInputField(element)
}

function makeEditCategoryVisible() {
	let element = document.getElementById("edit-category");

	ShowAndHideInputField(element);
}




