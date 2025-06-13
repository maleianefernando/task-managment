function onLoginSubmit() {
  u("form.login").on("submit", function (e) {
    router.navigate("/tasks/list");
  });
}

async function taskRegisterScripts() {
  const response = await getUsers();
  const users = Object.values(response).filter(item => typeof item === "object" && item !== null && !("status" in item));
  
  users.forEach(user => {
    u('form.register .workers').append(
      `
        <option value="${user.id}">${user.name}</option>
      `
    );

  });
  u('form.register .workers').on('change', async function () {
    const workerId = u('form.register .workers').first().value;
    const worker = await fetchData(`/workers/${workerId}`);
    const workerInput =  u('.worker-input-json').first().value = JSON.stringify(worker);
    console.log(workerInput)
  })
  registerScripts("/tasks", "Tarefa registada com sucesso");
}

function registerScripts(endpoint, successMsg) {
  u("form.register").on("submit", async function (e) {
    e.preventDefault();
    const form = new FormData(this);

    const data = Object.fromEntries(form.entries());
    console.log(data);
    const response = await postData(endpoint, data);
    if (response.status == 201) {
      u("form.register").before(alert("success", "check-circle", successMsg));
    } else {
      const errors = response.data.errors;
      const key = Object.keys(errors)[0];
      console.log(errors);
      u("form.register").before(
        alert("danger", "exclamation-octagon", errors[key])
      );
    }
    setTimeout((e) => {
      u("div.alert").remove();
    }, 2500);
  });
}

function alert(type, bIcon, message) {
  return `
  <div class="alert alert-${type} alert-dismissible fade show" role="alert">
    <i class="bi bi-${bIcon} me-1"></i>
    ${message}
    <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
  </div>
  `;
}

async function getUsers() {
  const response = await fetchData("/workers");
  return response;
}
