const currentPath = window.location.pathname.replace(/\/$/, "") || "/";

document.querySelectorAll(".site-header__nav a").forEach((link) => {
  const linkPath = new URL(link.href, window.location.origin).pathname.replace(/\/$/, "") || "/";
  if (linkPath === currentPath) {
    link.classList.add("is-active");
    link.setAttribute("aria-current", "page");
  }
});
