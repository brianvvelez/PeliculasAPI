import { Link } from 'react-router-dom';

export default function Home() {
  const modules = [
    { name: 'Géneros', path: '/generos', icon: '🎭', desc: 'Gestionar géneros de películas y series' },
    { name: 'Directores', path: '/directores', icon: '🎬', desc: 'Gestionar directores' },
    { name: 'Productoras', path: '/productoras', icon: '🏢', desc: 'Gestionar productoras' },
    { name: 'Tipos', path: '/tipos', icon: '📋', desc: 'Gestionar tipos de contenido' },
    { name: 'Media', path: '/media', icon: '🎥', desc: 'Gestionar películas y series' },
  ];

  return (
    <div className="container mt-4">
      <div className="text-center mb-4">
        <h1>Películas y Series</h1>
        <p className="text-muted">Sistema de gestión de contenido multimedia</p>
      </div>
      <div className="row g-3">
        {modules.map((m) => (
          <div key={m.path} className="col-md-4 col-lg-3">
            <Link to={m.path} className="text-decoration-none">
              <div className="card h-100 text-center shadow-sm">
                <div className="card-body">
                  <div style={{ fontSize: '2.5rem' }}>{m.icon}</div>
                  <h5 className="card-title mt-2">{m.name}</h5>
                  <p className="card-text text-muted small">{m.desc}</p>
                </div>
              </div>
            </Link>
          </div>
        ))}
      </div>
    </div>
  );
}
