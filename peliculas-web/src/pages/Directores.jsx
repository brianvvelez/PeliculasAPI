import { useState, useEffect } from 'react';
import { directorService } from '../services/api';
import Alert from '../components/Alert';

const emptyForm = { nombres: '', estado: true };

export default function Directores() {
  const [items, setItems] = useState([]);
  const [form, setForm] = useState(emptyForm);
  const [editing, setEditing] = useState(null);
  const [error, setError] = useState('');
  const [success, setSuccess] = useState('');
  const [loading, setLoading] = useState(false);

  const load = async () => {
    setLoading(true);
    try {
      const { data } = await directorService.getAll();
      setItems(data);
    } catch {
      setError('Error al cargar directores');
    }
    setLoading(false);
  };

  useEffect(() => { load(); }, []);

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError(''); setSuccess('');
    try {
      if (editing) {
        await directorService.update(editing, form);
        setSuccess('Director actualizado');
      } else {
        await directorService.create(form);
        setSuccess('Director creado');
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
    setForm({ nombres: item.nombres, estado: item.estado });
    setError(''); setSuccess('');
  };

  const handleDelete = async (id) => {
    if (!confirm('¿Eliminar este director?')) return;
    try {
      const { data } = await directorService.delete(id);
      setSuccess(data.mensaje || 'Director eliminado');
      load();
    } catch (e) {
      setError(e.response?.data?.mensaje || 'Error al eliminar');
    }
  };

  const cancel = () => { setForm(emptyForm); setEditing(null); };

  return (
    <div className="container mt-4">
      <h2>Directores</h2>
      <Alert message={error} type="danger" onClose={() => setError('')} />
      <Alert message={success} type="success" onClose={() => setSuccess('')} />

      <div className="card mb-4">
        <div className="card-header">{editing ? 'Editar Director' : 'Nuevo Director'}</div>
        <div className="card-body">
          <form onSubmit={handleSubmit}>
            <div className="row g-3">
              <div className="col-md-5">
                <label className="form-label">Nombres *</label>
                <input className="form-control" required maxLength={200}
                  value={form.nombres} onChange={(e) => setForm({ ...form, nombres: e.target.value })} />
              </div>
              <div className="col-md-3">
                <label className="form-label">Estado</label>
                <select className="form-select" value={form.estado}
                  onChange={(e) => setForm({ ...form, estado: e.target.value === 'true' })}>
                  <option value="true">Activo</option>
                  <option value="false">Inactivo</option>
                </select>
              </div>
              <div className="col-md-4 d-flex align-items-end gap-2">
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
            <tr><th>ID</th><th>Nombres</th><th>Estado</th><th>Fecha Creación</th><th>Acciones</th></tr>
          </thead>
          <tbody>
            {items.map((item) => (
              <tr key={item.id}>
                <td>{item.id}</td>
                <td>{item.nombres}</td>
                <td><span className={`badge bg-${item.estado ? 'success' : 'secondary'}`}>{item.estadoTexto}</span></td>
                <td>{new Date(item.fechaCreacion).toLocaleDateString()}</td>
                <td>
                  <button className="btn btn-sm btn-warning me-1" onClick={() => handleEdit(item)}>Editar</button>
                  <button className="btn btn-sm btn-danger" onClick={() => handleDelete(item.id)}>Eliminar</button>
                </td>
              </tr>
            ))}
            {items.length === 0 && <tr><td colSpan={5} className="text-center">No hay directores</td></tr>}
          </tbody>
        </table>
      )}
    </div>
  );
}
