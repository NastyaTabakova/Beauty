function updateMasterInfo() {
    var data = $("#infoForUpdate").serialize();
    $.ajax({
        type: 'POST',
        url: '/MasterPages/updateMasterInfo',
        data: data,
        success: function () {
            swal("Данные обновлены!", " ", "success");
        },
        error: function () {
            swal("Данные не обновлены!", "Попробуйте еще раз", "error");
        }

    });
}

function updateMasterPassword() {
    var data = $("#passForUpdate").serialize();
    $.ajax({
        type: 'POST',
        url: '/MasterPages/updateMasterPassword',
        data: data,
        success: function () {
            swal("Данные обновлены!", " ", "success");
        },
        error: function () {
            swal("Данные не обновлены!", "Попробуйте еще раз", "error");
        }

    });
}

function updateUserInfo() {
    var data = $("#infoForUpdate").serialize();
    $.ajax({
        type: 'POST',
        url: '/UserPages/updateUserInfo',
        data: data,
        success: function () {
            swal("Данные обновлены!", " ", "success");
        },
        error: function () {
            swal("Данные не обновлены!", "Попробуйте еще раз", "error");
        }

    });
}
function updateUserPassword() {
    var data = $("#passForUpdate").serialize();
    $.ajax({
        type: 'POST',
        url: '/UserPages/updateUserPassword',
        data: data,
        success: function () {
            swal("Данные обновлены!", " ", "success");
        },
        error: function () {
            swal("Данные не обновлены!", "Попробуйте еще раз", "error");
        }

    });
}


function getTime(obj) {
    try {
        $("#times").remove();
    }
    catch {

    }
    $.ajax({
        type: 'GET',
        url: '/UserPages/getTimes',
        data: {
            Id: obj.id,
            Date : obj.value
        },
        dataType: "json",
        success: function (data) {
            var div = document.createElement("div");
            div.id = "times";
            div.className = "row";
            $.each(data, function (i, item) {
                div.innerHTML += '<div class="col">' +
                                    '<button class="btn btn-outline-secondary rounded-pill" id="' + obj.id + '" onclick="updateService(this)" type="button" value = "' + item +'">' + item +'</button>'
                                '</div > ';
            });
            document.getElementById("idmas").before(div);
        },
        error: function () {
            swal("Данные о времени не получены!", "Попробуйте выбрать другую дату", "error");
        }
    })
}

function updateService(obj) {
    $.ajax({
        type: 'POST',
        url: '/UserPages/CheckService',
        data: {
            idmaster: obj.id,
            date: document.getElementsByName("date")[0].value,
            time: obj.value,
            price: document.getElementById("price").value,
            subid: document.getElementById("subid").value
        },
        success: function () {
            swal("Успешно!", "Вы записаны, данные о записи можно увидеть на странице Записи", "success");
        },
        error: function () {
            swal("Что-то пошло не так!", "Вы не записаны, попробуйте еще раз", "error");
        }
    })
}


function getSubcategory(obj) {
    try {
        $("#subcategories").remove();
    }
    catch {

    }
    $.ajax({
        type: 'GET',
        url: '/MasterPages/getSubcategory',
        data: {
            catid: document.getElementById("category").value,
        },
        dataType: "json",
        success: function (data) {
            var select = document.createElement("select");
            select.id = "subcategories";
            select.name = "subc";
            select.className = "form-control rounded-pill"
            select.style="margin-top:10px "
            $.each(data, function (i, item) {
                select.innerHTML += 
                    '<option value = "' + item + '">' + item + '</option>'
            });
            document.getElementById("category").after(select);
        },
        error: function () {
            swal("Данные о подкатегории не получены!", "Попробуйте выбрать другую категорию", "error");
        }
    })
}


function addService() {
    $.ajax({
        type: 'POST',
        url: '/MasterPages/CreateNewService',
        data: {
            subc: document.getElementById("subcategories").value,
            price: document.getElementById("price").value,
            discription: document.getElementsByName("discription")[0].value,
        },
        success: function () {
            swal("Услуга добавлена", " ", "success");
        },
        error: function () {
            swal("Услуга не добавлена", "Попробуйте еще раз", "error");
        }
    })
}


function addPhoto() {
    var formData = new FormData();
    formData.append('photo', $('input[type=file]')[0].files[0]);
    formData.append('descr', document.getElementById('descr').value)
    $.ajax({
        type: 'POST',
        url: '/MasterPages/addPhoto',
        data: formData,
        processData: false,
        contentType: false,
        success: function () {
            swal("Фото добавлено", " ", "success");
        },
        error: function () {
            swal("Фото не добавлено", "Попробуйте еще раз", "error");
        }

    });
}

function addShedul() {
    var data = $("#shedInfo").serialize();
    $.ajax({
        type: 'POST',
        url: '/MasterPages/addShedul',
        data: data,
        success: function () {
            swal("Дата и время работы добавлены", " ", "success");
        },
        error: function () {
            swal("Дата и время работы  не добавлены", "Попробуйте еще раз", "error");
        }

    });
}

function Cancel() {
    var data = $("#cancel").serialize();
    $.ajax({
        type: 'POST',
        url: '/UserPages/cancelEntry',
        data: data,
        success: function () {
            swal("Успешно", "Вы отменили запись", "success");
        },
        error: function () {
            swal("Что-то пошло не так", "Попробуйте еще раз или позвоните мастеру для отмены записи", "error");
        }

    });
}
function CancelMas() {
    var data = $("#CancelMas").serialize();
    $.ajax({
        type: 'POST',
        url: '/MasterPages/cancelEntry',
        data: data,
        success: function () {
            swal("Успешно", "Вы отменили запись", "success");
        },
        error: function () {
            swal("Что-то пошло не так", "Попробуйте еще раз", "error");
        }

    });
}

function OkMas() {
    var data = $("#OkMas").serialize();
    $.ajax({
        type: 'POST',
        url: '/MasterPages/OkEntry',
        data: data,
        success: function () {
            swal("Успешно", "Вы подтвердили выполнение услуги", "success");
        },
        error: function () {
            swal("Что-то пошло не так", "Попробуйте еще раз", "error");
        }

    });
}

function insertCategory() {
    var formData = new FormData();
    formData.append('photo', $('input[type=file]')[0].files[0]);
    formData.append('categoryName', document.getElementById('categoryName').value)
    $.ajax({
        type: 'POST',
        url: '/Admin/InsertCategory',
        data: formData,
        processData: false,
        contentType: false,
        success: function () {
            swal("Добавлено", " ", "success");
        },
        error: function () {
            swal("Не добавлено", "Попробуйте еще раз", "error");
        }

    });
}


function insertSubcategory() {
    var formData = new FormData();
    formData.append('photo', $('input[type=file]')[0].files[0]);
    formData.append('categId', document.getElementById('categId').value)
    formData.append('subcategoryName', document.getElementById('subcategoryName').value)
    $.ajax({
        type: 'POST',
        url: '/Admin/InsertSubcategory',
        data: formData,
        processData: false,
        contentType: false,
        success: function () {
            swal("Добавлено", " ", "success");
        },
        error: function () {
            swal("Не добавлено", "Попробуйте еще раз", "error");
        }

    });
}

function updateCategory() {
    var formData = new FormData();
    formData.append('categId', document.getElementById('categId').value)
    formData.append('newCateg', document.getElementById('newCateg').value)
    formData.append('photo', $('input[type=file]')[0].files[0]);
    $.ajax({
        type: 'PUT',
        url: '/Admin/UpdateCategory',
        data: formData,
        processData: false,
        contentType: false,
        success: function () {
            swal("Добавлено", " ", "success");
        },
        error: function () {
            swal("Не добавлено", "Попробуйте еще раз", "error");
        }

    });
}
function updateSubcategory() {
    var formData = new FormData();
    formData.append('categId', document.getElementById('categId').value)
    formData.append('newCateg', document.getElementById('newCateg').value)
    formData.append('photo', $('input[type=file]')[0].files[0]);
    $.ajax({
        type: 'PUT',
        url: '/Admin/UpdateSubcategory',
        data: formData,
        processData: false,
        contentType: false,
        success: function () {
            swal("Добавлено", " ", "success");
        },
        error: function () {
            swal("Не добавлено", "Попробуйте еще раз", "error");
        }

    });
}


function DeleteCategory() {
    $.ajax({
        type: 'DELETE',
        url: '/Admin/DeleteCategory',
        data: {
            categId: document.getElementById("categId").value,
        },
        success: function () {
            swal("Успешно", " ", "success");
        },
        error: function () {
            swal("Не успешно", "Вероятно в этой категории есть подкатегории", "error");
        }
    })
}

function DeleteCategory() {
    $.ajax({
        type: 'DELETE',
        url: '/Admin/DeleteCategory',
        data: {
            categId: document.getElementById("categId").value,
        },
        success: function () {
            swal("Успешно", " ", "success");
        },
        error: function () {
            swal("Не успешно", "Вероятно в этой категории есть подкатегории", "error");
        }
    })
}

function DeleteSubcategory() {
    var formData = new FormData();
    formData.append('subId', document.getElementById('subId').value)
    $.ajax({
        type: 'DELETE',
        url: '/Admin/DeleteSubCategory',
        data: formData,
        processData: false,
        contentType: false,
        success: function () {
            swal("Успешно", " ", "success");
        },
        error: function () {
            swal("Не успешно", "Вероятно в этой категории есть услуги", "error");
        }

    });
}

function Auth() {
    var formData = new FormData();
    formData.append('email', document.getElementById('email').value)
    formData.append('password', document.getElementById('password').value)
    $.ajax({
        type: 'POST',
        url: '/RegistrationAndAuthorization/Authorization',
        data: formData,
        processData: false,
        contentType: false,
        success: function () {
           
        },
        error: function () {
            swal("Не удалось войти :(", "Проверьте даннные для входа и попробуйте еще раз", "error");
        }

    });
}

function DelService() {
    var formData = new FormData();
    formData.append('id', document.getElementById('id').value)
    $.ajax({
        type: 'POST',
        url: '/MasterPages/DelService',
        data: formData,
        processData: false,
        contentType: false,
        success: function () {
            swal("Вы удалили услугу", "Клиенты больше не смогут на нее записаться", "success");
        },
        error: function () {
            swal("Не удалось удалить услугу", "Попробуйте позже", "error");
        }

    });
}
