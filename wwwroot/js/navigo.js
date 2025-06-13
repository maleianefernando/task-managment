const router = new Navigo("/", { hash: true });
const ROUTES = {
  ROOT: "/",
  HOME: "home",
  LOGIN: "login",
  LOGOUT: "logout",
  WORKERS: {
    REGISTER: "workers/register",
    SHOW: "workers/show",
    LIST: "workers/list",
  },
  TASKS: {
    REGISTER: "tasks/register",
    SHOW: "tasks/show",
    LIST: "tasks/list",
  },
};

router.hooks({
  before: (done, match) => {
    // console.log(match);
    (function clearUrl() {
      const path =
        window.location.pathname + window.location.hash.split("?")[0];
      history.replaceState(null, "", path);
      done();
    })();
  },
});

router.on(ROUTES.LOGIN, async function (r) {
  sessionStorage.setItem("route", r.route.path);
  u("aside").html("");
  u("header").html("");
  const html = await retrieveHTML(`login.html`);
  u("main").html(html);

  onLoginSubmit();
});

router.on(ROUTES.LOGOUT, async function (r) {
  router.navigate("login");
});

router.on(ROUTES.HOME, async function (r) {
  sessionStorage.setItem("route", r.route.path);
  await setAside();
  const mainHtml = await retrieveHTML(`tasks/register.html`);
  u("main").attr("id", "main").addClass("main").html(mainHtml);
});

(function taskRoutes() {
  const taskDir = `tasks`;
  router.on(ROUTES.TASKS.REGISTER, async function (r) {
    sessionStorage.setItem("route", r.route.path);
    await setAside();
    const mainHtml = await retrieveHTML(`${taskDir}/register.html`);
    u("main").attr("id", "main").addClass("main").html(mainHtml);

    taskRegisterScripts();
  });

  router.on(ROUTES.TASKS.LIST, async function (r) {
    sessionStorage.setItem("route", r.route.path);
    await setAside();
    const mainHtml = await retrieveHTML(`${taskDir}/list.html`);
    u("main").attr("id", "main").addClass("main").html(mainHtml);
  });

  router.on(ROUTES.TASKS.SHOW, async function (r) {
    sessionStorage.setItem("route", r.route.path);
    await setAside();
    const mainHtml = await retrieveHTML(`${taskDir}/show.html`);
    u("main").attr("id", "main").addClass("main").html(mainHtml);
  });
})();

(function workerRoutes() {
  const workerDir = `workers`;
  router.on(ROUTES.WORKERS.REGISTER, async function (r) {
    sessionStorage.setItem("route", r.route.path);
    await setAside();
    const mainHtml = await retrieveHTML(`${workerDir}/register.html`);
    u("main").attr("id", "main").addClass("main").html(mainHtml);

    registerScripts('/workers', 'Funcionário registado com sucesso');
  });

  router.on(ROUTES.WORKERS.LIST, async function (r) {
    sessionStorage.setItem("route", r.route.path);
    console.log("worker");
    await setAside();
    const mainHtml = await retrieveHTML(`${workerDir}/list.html`);
    u("main").attr("id", "main").addClass("main").html(mainHtml);
  });

  router.on(ROUTES.WORKERS.SHOW, async function (r) {
    sessionStorage.setItem("route", r.route.path);
    await setAside();
    const mainHtml = await retrieveHTML(`${workerDir}/show.html`);
    u("main").attr("id", "main").addClass("main").html(mainHtml);
  });
})();

router.resolve();

async function retrieveHTML(filePath, dir = "components") {
  const fullFilePath = `${dir}/${filePath}`;
  try {
    const response = await fetch(fullFilePath);
    if (!response.ok) throw new Error("Error requesting component data");
    return await response.text();
  } catch (error) {
    console.log(error);
  }
}

async function setAside() {
  const sidebarHtml = await retrieveHTML(`sidebar.html`);
  const headerHtml = await retrieveHTML(`header.html`);

  u("header")
    .attr("id", "header")
    .addClass("header fixed-top d-flex align-items-center")
    .html(headerHtml);
  u("aside").attr("id", "sidebar").addClass("sidebar").html(sidebarHtml);
}

window.onload = function () {
  // history.replaceState(null, "", "");
  const route = sessionStorage.getItem("route");
  route ? router.navigate(`${route}`) : router.navigate(ROUTES.LOGIN);
};
