import { NavLink } from 'react-router-dom';

export default function Navbar() {
  return (
    <nav className="navbar navbar-expand-lg navbar-dark bg-dark">
      <div className="container">
        <NavLink className="navbar-brand" to="/">
          PeliculasApp
        </NavLink>
        <button
          className="navbar-toggler"
          type="button"
          data-bs-toggle="collapse"
          data-bs-target="#navbarNav"
        >
          <span className="navbar-toggler-icon"></span>
        </button>
        <div className="collapse navbar-collapse" id="navbarNav">
          <ul className="navbar-nav">
            <li className="nav-item">
              <NavLink className="nav-link" to="/generos">
                Géneros
              </NavLink>
            </li>
            <li className="nav-item">
              <NavLink className="nav-link" to="/directores">
                Directores
              </NavLink>
            </li>
            <li className="nav-item">
              <NavLink className="nav-link" to="/productoras">
                Productoras
              </NavLink>
            </li>
            <li className="nav-item">
              <NavLink className="nav-link" to="/tipos">
                Tipos
              </NavLink>
            </li>
            <li className="nav-item">
              <NavLink className="nav-link" to="/media">
                Media
              </NavLink>
            </li>
          </ul>
        </div>
      </div>
    </nav>
  );
}
