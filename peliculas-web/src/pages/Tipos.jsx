import { useState, useEffect } from 'react';
import { tipoService } from '../services/api';
import Alert from '../components/Alert';

const emptyForm = { nombre: '', descripcion: '' };

export default function Tipos() {
  const [items, setItems] = useState([]);
  const [form, setForm] = useState(emptyForm);
  const [editing, setEditing] = useState(null);
  const [error, setError] = useState('');
  const [success, setSuccess] = useState('');
  const [loading, setLoading] = useState(false);

  const load = async () => {
    setLoading(true);
    try {
      const { data } = await tipoService.getAll();
      setItems(data);
    } catch {
      setError('Error al cargar tipos');
    }
    setLoading(false);
  };

  useEffect(() => { load(); }, []);

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError(''); setSuccess('');
    try {
      if (editing) {
        await tipoService.update(editing, form);
        setSuccess('Tipo actualizado');
      } else {
        await tipoService.create(form);
        setSuccess('Tipo creado');
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
    setForm({ nombre: item.nombre, descripcion: item.descripcion || '' });
    setError(''); setSuccess('');
  };

  const handleDelete = async (id) => {
    if (!confirm('¿Eliminar este tipo?')) return;
    try {
      const { data } = await tipoService.delete(id);
      setSuccess(data.mensaje || 'Tipo eliminado');
      load();
    } catch (e) {
      setError(e.response?.data?.mensaje || 'Error al eliminar');
    }
  };

  const cancel = () => { setForm(emptyForm); setEditing(null); };

  return (
    <div className="container mt-4">
      <h2>Tipos</h2>
      <Alert message={error} type="danger" onClose={() => setError('')} />
      <Alert message={success} type="success" onClose={() => setSuccess('')} />

      <div className="card mb-4">
        <div className="card-header">{editing ? 'Editar Tipo' : 'Nuevo Tipo'}</div>
        <div className="card-body">
          <form onSubmit={handleSubmit}>
            <div className="row g-3">
              <div className="col-md-4">
                <label className="form-label">Nombre *</label>
                <input className="form-control" required maxLength={100}
                  value={form.nombre} onChange={(e) => setForm({ ...form, nombre: e.target.value })} />
              </div>
              <div className="col-md-5">
                <label className="form-label">Descripción</label>
                <input className="form-control" maxLength={500}
                  value={form.descripcion} onChange={(e) => setForm({ ...form, descripcion: e.target.value })} />
              </div>
              <div className="col-md-3 d-flex align-items-end gap-2">
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
            <tr><th>ID</th><th>Nombre</th><th>Descripción</th><th>Fecha Creación</th><th>Acciones</th></tr>
          </thead>
          <tbody>
            {items.map((item) => (
              <tr key={item.id}>
                <td>{item.id}</td>
                <td>{item.nombre}</td>
                <td>{item.descripcion}</td>
                <td>{new Date(item.fechaCreacion).toLocaleDateString()}</td>
                <td>
                  <button className="btn btn-sm btn-warning me-1" onClick={() => handleEdit(item)}>Editar</button>
                  <button className="btn btn-sm btn-danger" onClick={() => handleDelete(item.id)}>Eliminar</button>
                </td>
              </tr>
            ))}
            {items.length === 0 && <tr><td colSpan={5} className="text-center">No hay tipos</td></tr>}
          </tbody>
        </table>
      )}
    </div>
  );
}
