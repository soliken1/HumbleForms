//Dropdown Interaction
function selectedCourse(selection) {
    var courseddbtn = document.getElementById("courseddbtn");
    var course = document.getElementById("course");

    if (courseddbtn) {
        courseddbtn.textContent = selection;
        course.value = selection;
    }
}

function selectedYear(selection) {
    var yearddbtn = document.getElementById("yearddbtn");
    var year = document.getElementById("year");
    if (yearddbtn) {
        yearddbtn.textContent = selection;
        year.value = selection;
    }
}

function selectedRemarks(selection) {
    var remarksddbtn = document.getElementById("remarksddbtn");
    var remarks = document.getElementById("remarks");
    if (remarksddbtn) {
        remarksddbtn.textContent = selection;
        remarks.value = selection;
    }
}

function selectedOffering(selection) {
    var offeringddbtn = document.getElementById("offeringddbtn");
    var offering = document.getElementById("offering");
    if (offeringddbtn) {
        offeringddbtn.textContent = selection;
        offering.value = selection;
    }
}

function selectedCategory(selection) {
    var categoryddbtn = document.getElementById("categoryddbtn");
    var category = document.getElementById("category");
    if (categoryddbtn) {
        categoryddbtn.textContent = selection;
        category.value = selection;
    }
}