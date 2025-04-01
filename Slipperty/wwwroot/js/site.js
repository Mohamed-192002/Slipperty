function previewImage(fileUploadId, imgId) {
    const imageInput = document.getElementById(fileUploadId);
    const imagePreview = document.getElementById(imgId);

    if (imageInput.files && imageInput.files[0]) {
        imagePreview.src = window.URL.createObjectURL(imageInput.files[0]);
    }
}


$(document).on({
    keypress: function (event) {
        // Allow digits (48-57), minus sign (45) at the beginning, and the dot (46) if not already present
        if (
            (event.which === 45 && $(this).val().indexOf('-') === -1) ||  // Allow minus sign at the beginning only
            (event.which >= 48 && event.which <= 57) ||  // Allow digits (0-9)
            (event.which === 46 && $(this).val().indexOf('.') === -1)  // Allow dot (.) if not already present
        ) {
            return; // Allow the keypress
        }
        event.preventDefault();  // Prevent the keypress if it's not valid
    },
    change: function () {
        // Get the value as a number
        var value = parseFloat($(this).val());

        //// If the value is negative (less than 0), allow it to stay as negative
        //if (value < 0) {
        //    return;  // Don't change anything if it's negative
        //}

        //// If the value is positive or zero, reset to zero
        //$(this).val("0");
    }
}, ".onlyNumbers");

$(document).on({
    keypress: function (event) {
        // Allow English letters (A-Z, a-z), digits (0-9), space (32), dash (-), and underscore (_)
        if (
            (event.which >= 65 && event.which <= 90) ||  // Uppercase letters (A-Z)
            (event.which >= 97 && event.which <= 122) || // Lowercase letters (a-z)
            (event.which >= 48 && event.which <= 57) ||  // Digits (0-9)
            (event.which === 32) ||  // Space (32)
            (event.which === 45) ||  // Dash (-)
            (event.which === 95)     // Underscore (_)
        ) {
            return; // Allow the keypress
        }
        event.preventDefault();  // Prevent the keypress if it's not valid
    },
    change: function () {
        // You can add further logic here if needed
    }
}, ".onlyEnglish");


function search(inputId, tableId, fieldName) {
    var input, filter, table, tr, tds, td, i, j, txtValue, found;
    input = document.getElementById(inputId);
    filter = input.value.toUpperCase();
    table = document.getElementById(tableId);
    tr = table.getElementsByTagName("tr");
    for (i = 0; i < tr.length; i++) {
        found = false;
        tds = tr[i].getElementsByClassName(fieldName);
        for (j = 0; j < tds.length; j++) {
            td = tds[j];
            if (td) {
                txtValue = td.textContent || td.innerText;
                if (txtValue.toUpperCase().indexOf(filter) > -1) {
                    tr[i].style.display = "";
                    found = true;
                    break;
                } else {
                    tr[i].style.display = "none";
                }
            }

        }
        if (found == true) {
            continue;
        }
    }
}



function rowCalc() {
    // Loop through each table
    $('table').each(function () {
        var count = 0;
        var table = $(this);

        // Loop through only rows within the current table and find '.rowNumber'
        table.find('.rowNumber').each(function (i) {
            count = i + 1;  // Update row number starting from 1
            $(this).text(count);  // Set the row number text to the current count
        });
    });
}


function updateSelect2() {
    $('.select2').select2();
}

$(document).ready(function () {
    // Function to go to the next tab
    function goToNextTab() {
        // Find the current active tab link
        var currentTab = $('.nav-tabs .active');

        // Find the index of the current tab
        var currentIndex = $('.nav-tabs .nav-link').index(currentTab);

        // Find the next tab link
        var nextTab = $('.nav-tabs .nav-link').eq(currentIndex + 1);

        // If the next tab exists, switch to it
        if (nextTab.length) {
            // Remove active class from the current tab and content
            currentTab.removeClass('active');
            $('.tab-pane').removeClass('show active');

            // Add active class to the next tab and its content
            nextTab.addClass('active');
            $('#' + nextTab.attr('href').substring(1)).addClass('show active');
        }
    }

    // Function to go to the previous tab
    function goToPreviousTab() {
        // Find the current active tab link
        var currentTab = $('.nav-tabs .active');

        // Find the index of the current tab
        var currentIndex = $('.nav-tabs .nav-link').index(currentTab);

        // Find the previous tab link
        var previousTab = $('.nav-tabs .nav-link').eq(currentIndex - 1);

        // If the previous tab exists, switch to it
        if (previousTab.length) {
            // Remove active class from the current tab and content
            currentTab.removeClass('active');
            $('.tab-pane').removeClass('show active');

            // Add active class to the previous tab and its content
            previousTab.addClass('active');
            $('#' + previousTab.attr('href').substring(1)).addClass('show active');
        }
    }

    // Use the function when a "continue" button is clicked
    $('.continue').click(function () {
        goToNextTab(); // Go to the next tab
        updateSelect2();
    });

    // Use the function when a "back" button is clicked
    $('.back').click(function () {
        goToPreviousTab(); // Go to the previous tab
        updateSelect2();
    });

    $('#sidebar').toggleClass('active'); // Remove 'active' class on page load

    $('#sidebar').on('transitionend', function () {
        updateSelect2();
    });
    $('#nav-tab a').on('click', function (e) {
        // Get the newly active tab
        var activeTab = $(e.target); // Activated tab
        // Call your specific function when the tab is activated
        setTimeout(function () {
            updateSelect2();
        }, 250);
    });


    // Bind the 'select2:open' event for all select2 dropdowns
    $('.select2').on('select2:open', function () {
        // Focus the search input of the currently opened select2 dropdown
        var searchField = $(this).data('select2').$dropdown.find('.select2-search__field');
        setTimeout(function () {
            searchField.focus();
        }, 100);  // Adding a slight delay ensures the dropdown is fully opened before focusing
    });


});

function handleProductImages(files) {
    const mainImageSelector = document.getElementById("mainImageSelector");
    //const iconImageSelector = document.getElementById("IconImageSelector");

    // Loop through the files and add them to both divs
    Array.from(files).forEach(file => {
        const img = document.createElement("img");
        const imageUrl = URL.createObjectURL(file);  // Create a URL for the image file
        img.src = imageUrl;
        img.classList.add("formImg", "img-thumbnail", "form-img-thumbnail", "d-block");

        // Create a container for each image so we can position the remove button
        const imgContainerForMain = document.createElement("div");
        imgContainerForMain.classList.add("img-container");
        imgContainerForMain.dataset.imageUrl = imageUrl;  // Store the image URL in the container
        imgContainerForMain.dataset.fileName = file.name; // Store the file name in the container

        const imgContainerForIcon = document.createElement("div");
        imgContainerForIcon.classList.add("img-container");
        imgContainerForIcon.dataset.imageUrl = imageUrl;  // Store the image URL in the container
        imgContainerForIcon.dataset.fileName = file.name; // Store the file name in the container

        // Create remove buttons
        const removeButtonForMain = createRemoveButton(file.name, imgContainerForMain, "main");
        const removeButtonForIcon = createRemoveButton(file.name, imgContainerForIcon, "icon");

        // Add image and remove button to their containers
        imgContainerForMain.appendChild(img.cloneNode());
        imgContainerForMain.appendChild(removeButtonForMain);

        imgContainerForIcon.appendChild(img.cloneNode());
        imgContainerForIcon.appendChild(removeButtonForIcon);

        // Append the image containers to their respective divs
        mainImageSelector.appendChild(imgContainerForMain);
        //iconImageSelector.appendChild(imgContainerForIcon);

        // Add click event on image containers to set the main and icon images
        addImageClickEvent(imgContainerForMain, file.name, mainImageSelector, "main");
        //addImageClickEvent(imgContainerForIcon, file.name, iconImageSelector, "icon");
    });
}

// Function to create a remove button and add an event listener to it
function createRemoveButton(fileName, container, type) {
    const removeButton = document.createElement("button");
    removeButton.classList.add("remove-btn", "btn", "btn-danger", "form-remove-btn", "w-100");
    removeButton.textContent = "حذف";

    // Add click event to remove the image from the div
    removeButton.addEventListener("click", function (event) {
        event.stopPropagation();  // Stop the event from bubbling up to the image container
        removeImageFromDivs(fileName);  // Use the file name to identify the image
        removeImageFromOrderTable(fileName);  // Use the file name to identify the image
        removeImageFromColorImageModal(fileName);  // Use the file name to identify the image
        container.remove();  // Remove the image container
    });

    return removeButton;
}

// Function to add click event for setting main or icon image and selecting borders
function addImageClickEvent(container, fileName, selector, type) {
    container.addEventListener("click", function () {
        if (type === "main") {
            setMainImage(fileName);  // Set the name of the image in the input field for main image
        } else {
            setIconImage(fileName);  // Set the name of the image in the input field for icon image
        }
        setSelectedBorder(container, selector);  // Set the selected border for the image
    });
}

// Placeholder for the removeImageFromDivs function
function removeImageFromDivs(fileName) {
    // Implement the logic for removing the image
}

// Placeholder for the setMainImage function
function setMainImage(fileName) {
    // Implement the logic for setting the main image
}

// Placeholder for the setIconImage function
function setIconImage(fileName) {
    // Implement the logic for setting the icon image
}



function removeImageFromDivs(fileName) {
    // Select all image containers from both divs
    const mainImages = document.querySelectorAll("#mainImageSelector .img-container");
    //const iconImages = document.querySelectorAll("#IconImageSelector .img-container");

    // Remove the image container if its file name matches the deleted image's name
    mainImages.forEach(imgContainer => {
        if (imgContainer.dataset.fileName === fileName) {
            imgContainer.remove();  // Remove from the main image div
        }
    });

    //iconImages.forEach(imgContainer => {
    //    if (imgContainer.dataset.fileName === fileName) {
    //        imgContainer.remove();  // Remove from the icon image div
    //    }
    //});

    // After removal, check if the deleted image was the selected image and clear the input if needed
    const mainImageInput = document.getElementById("MainImageUrl");
    if (mainImageInput.value === fileName) {
        mainImageInput.value = "";  // Clear the main image input if the deleted image was selected
    }

    const iconImageInput = document.getElementById("IconImageUrl");
    if (iconImageInput.value === fileName) {
        iconImageInput.value = "";  // Clear the icon image input if the deleted image was selected
    }
}

// Function to handle setting the main image
function setMainImage(imageName) {
    const mainImageInput = document.getElementById("MainImageUrl");
    mainImageInput.value = imageName;  // Set the input field value to the image name
}

// Function to handle setting the icon image
function setIconImage(imageName) {
    const iconImageInput = document.getElementById("IconImageUrl");
    iconImageInput.value = imageName;  // Set the input field value to the image name
}

// Function to set the selected border when an image container is clicked
function setSelectedBorder(container, selectorContainer) {
    // Remove the 'selected' border from all containers within the specific selector container (either main or icon)
    const allContainers = selectorContainer.querySelectorAll(".img-container");
    allContainers.forEach(cont => {
        cont.classList.remove("selected-border");
    });

    // Add a 'selected' border to the clicked container
    container.classList.add("selected-border");
}


function deleteRow(button) {
    // Find the closest <tr> and remove it
    $(button).closest('tr').remove();
    rowCalc();
}

$('.preventSubmit').on('keydown', function (event) {
    if (event.key === "Enter") {
        event.preventDefault();  // Prevent form submission on Enter key press
    }
});


function ViewProduct(id) {
    var url = "/User/Products/ViewProduct?id=" + id;
    window.location.href = url;
}


let lastScrollTop = 0; // Variable to track the last scroll position
const navbar = document.querySelector('.topNavbar'); // Select the navbar element

window.addEventListener('scroll', function () {
    let currentScroll = window.pageYOffset || document.documentElement.scrollTop; // Get the current scroll position

    if (currentScroll > lastScrollTop) {
        // Scrolling down: hide the navbar
        navbar.classList.add('navbar-hidden');
    } else {
        // Scrolling up: show the navbar
        navbar.classList.remove('navbar-hidden');
    }

    lastScrollTop = currentScroll <= 0 ? 0 : currentScroll; // Update last scroll position
});




 // Function to get 'From' time
    function getDeliveryTimeFrom() {
        // Get values from the "From" time inputs
        var fromHour = $('#deliveryTimeFromHour').val();
        //var fromMinute = $('#deliveryTimeFromMinute').val();
        var fromMinute = '00';
        var fromAMPM = $('#deliveryTimeFromAMPM').val();

        // Validate if any of the values are provided, and create the time string
        if (fromHour || fromMinute || fromAMPM) {
            var fromTime = '';
            if (fromHour && fromMinute && fromAMPM) {
                fromTime = fromHour + ':' + fromMinute + ' ' + fromAMPM;
            } else {
                fromTime = '';
            }
            return fromTime;
        } else {
            return '';
        }
    }

    // Function to get 'To' time
    function getDeliveryTimeTo() {
        // Get values from the "To" time inputs
        var toHour = $('#deliveryTimeToHour').val();
        //var toMinute = $('#deliveryTimeToMinute').val();
        var toMinute = '00';
        var toAMPM = $('#deliveryTimeToAMPM').val();

        // Validate if any of the values are provided, and create the time string
        if (toHour || toMinute || toAMPM) {
            var toTime = '';
            if (toHour && toMinute && toAMPM) {
                toTime = toHour + ':' + toMinute + ' ' + toAMPM;
            } else {
                toTime = '';
            }
            return toTime;
        } else {
            return '';
        }
    }
/* Handle drap and drop to order images */
