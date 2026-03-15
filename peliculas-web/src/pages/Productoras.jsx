import { useState, useEffect } from 'react';
import { productoraService } from '../services/api';
import Alert from '../components/Alert';

const emptyForm = { nombre: '', estado: true, slogan: '', descripcion: '' };

export default function Productoras() {
  const [items, setItems] = useState([]);
  const [form, setForm] = useState(emptyForm);
  const [editing, setEditing] = useState(null);
  const [error, setError] = useState('');
  const [success, setSuccess] = useState('');
  const [loading, setLoading] = useState(false);

  const load = async () => {
    setLoading(true);
    try {
      const { data } = await productoraService.getAll();
      setItems(data);
    } catch {
      setError('Error al cargar productoras');
    }
    setLoading(false);
  };

  useEffect(() => { load(); }, []);

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError(''); setSuccess('');
    try {
      if (editing) {
        await productoraService.update(editing, form);
        setSuccess('Productora actualizada');
      } else {
        await productoraService.create(form);
        setSuccess('Productora creada');
      }
      setForm(emptyForm);
      setEditing(null);
      load();
    } catch (e) {
      setError(e.response?.data?.mensaje || e.response?.data?.title || 'Error al guardar');
    }
  };

  const handleEdit = (item) => {
    setEditing(item.id);
    setForm({ nombre: item.nombre, estado: item.estado, slogan: item.slogan || '', descripcion: item.descripcion || '' });
    setError(''); setSuccess('');
  };

  const handleDelete = async (id) => {
    if (!confirm('¿Eliminar esta productora?')) return;
    try {
      const { data } = await productoraService.delete(id);
      setSuccess(data.mensaje || 'Productora eliminada');
      load();
    } catch (e) {
      setError(e.response?.data?.mensaje || 'Error al eliminar');
    }
  };

  const cancel = () => { setForm(emptyForm); setEditing(null); };

  return (
    <div className="container mt-4">
      <h2>Productoras</h2>
      <Alert message={error} type="danger" onClose={() => setError('')} />
      <Alert message={success} type="success" onClose={() => setSuccess('')} />

      <div className="card mb-4">
        <div className="card-header">{editing ? 'Editar Productora' : 'Nueva Productora'}</div>
        <div className="card-body">
          <form onSubmit={handleSubmit}>
            <div className="row g-3">
              <div className="col-md-3">
                <label className="form-label">Nombre *</label>
                <input className="form-control" required maxLength={200}
                  value={form.nombre} onChange={(e) => setForm({ ...form, nombre: e.target.value })} />
              </div>
              <div className="col-md-3">
                <label className="form-label">Slogan</label>
                <input className="form-control" maxLength={300}
                  value={form.slogan} onChange={(e) => setForm({ ...form, slogan: e.target.value })} />
              </div>
              <div className="col-md-3">
                <label className="form-label">Descripción</label>
                <input className="form-control" maxLength={1000}
                  value={form.descripcion} onChange={(e) => setForm({ ...form, descripcion: e.target.value })} />
              </div>
              <div className="col-md-1">
                <label className="form-label">Estado</label>
                <select className="form-select" value={form.estado}
                  onChange={(e) => setForm({ ...form, estado: e.target.value === 'true' })}>
                  <option value="true">Activo</option>
                  <option value="false">Inactivo</option>
                </select>
              </div>
              <div className="col-md-2 d-flex align-items-end gap-2">
                <button type="submit" className="btn btn-primary">{editing ? 'Actualizar' : 'Crear'}</button>
                {editing && <button type="button" className="btn btn-secondary" onClick={cancel}>Cancelar</button>}
              </div>
            </div>
          </form>
        </div>
      </div>

      {loading ? <p>Cargando...</p> : (
        <table className="table table-striped">
          <thead>
            <tr><th>ID</th><th>Nombre</th><th>Slogan</th><th>Descripción</th><th>Estado</th><th>Acciones</th></tr>
          </thead>
          <tbody>
            {items.map((item) => (
              <tr key={item.id}>
                <td>{item.id}</td>
                <td>{item.nombre}</td>
                <td>{item.slogan}</td>
                <td>{item.descripcion}</td>
                <td><span className={`badge bg-${item.estado ? 'success' : 'secondary'}`}>{item.estadoTexto}</span></td>
                <td>
                  <button className="btn btn-sm btn-warning me-1" onClick={() => handleEdit(item)}>Editar</button>
                  <button className="btn btn-sm btn-danger" onClick={() => handleDelete(item.id)}>Eliminar</button>
                </td>
              </tr>
            ))}
            {items.length === 0 && <tr><td colSpan={6} className="text-center">No hay productoras</td></tr>}
          </tbody>
        </table>
      )}
    </div>
  );
}
