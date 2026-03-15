import { useState, useEffect } from 'react';
import { mediaService, generoService, directorService, productoraService, tipoService } from '../services/api';
import Alert from '../components/Alert';

const emptyForm = {
  serial: '', titulo: '', sinopsis: '', url: '', imagenPortada: '',
  anoEstreno: new Date().getFullYear(), generoId: '', directorId: '', productoraId: '', tipoId: '',
};

export default function MediaPage() {
  const [items, setItems] = useState([]);
  const [form, setForm] = useState(emptyForm);
  const [editing, setEditing] = useState(null);
  const [error, setError] = useState('');
  const [success, setSuccess] = useState('');
  const [loading, setLoading] = useState(false);
  const [generos, setGeneros] = useState([]);
  const [directores, setDirectores] = useState([]);
  const [productoras, setProductoras] = useState([]);
  const [tipos, setTipos] = useState([]);

  const loadCatalogs = async () => {
    try {
      const [g, d, p, t] = await Promise.all([
        generoService.getActivos(),
        directorService.getActivos(),
        productoraService.getActivas(),
        tipoService.getAll(),
      ]);
      setGeneros(g.data);
      setDirectores(d.data);
      setProductoras(p.data);
      setTipos(t.data);
    } catch {
      setError('Error al cargar catálogos');
    }
  };

  const load = async () => {
    setLoading(true);
    try {
      const { data } = await mediaService.getAll();
      setItems(data);
    } catch {
      setError('Error al cargar media');
    }
    setLoading(false);
  };

  useEffect(() => { loadCatalogs(); load(); }, []);

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError(''); setSuccess('');
    const payload = {
      ...form,
      anoEstreno: parseInt(form.anoEstreno),
      generoId: parseInt(form.generoId),
      directorId: parseInt(form.directorId),
      productoraId: parseInt(form.productoraId),
      tipoId: parseInt(form.tipoId),
    };
    try {
      if (editing) {
        await mediaService.update(editing, payload);
        setSuccess('Media actualizada');
      } else {
        await mediaService.create(payload);
        setSuccess('Media creada');
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
    setForm({
      serial: item.serial,
      titulo: item.titulo,
      sinopsis: item.sinopsis || '',
      url: item.url,
      imagenPortada: item.imagenPortada || '',
      anoEstreno: item.anoEstreno,
      generoId: item.generoId,
      directorId: item.directorId,
      productoraId: item.productoraId,
      tipoId: item.tipoId,
    });
    setError(''); setSuccess('');
  };

  const handleDelete = async (id) => {
    if (!confirm('¿Eliminar esta media?')) return;
    try {
      const { data } = await mediaService.delete(id);
      setSuccess(data.mensaje || 'Media eliminada');
      load();
    } catch (e) {
      setError(e.response?.data?.mensaje || 'Error al eliminar');
    }
  };

  const cancel = () => { setForm(emptyForm); setEditing(null); };

  return (
    <div className="container mt-4">
      <h2>Media (Películas y Series)</h2>
      <Alert message={error} type="danger" onClose={() => setError('')} />
      <Alert message={success} type="success" onClose={() => setSuccess('')} />

      <div className="card mb-4">
        <div className="card-header">{editing ? 'Editar Media' : 'Nueva Media'}</div>
        <div className="card-body">
          <form onSubmit={handleSubmit}>
            <div className="row g-3">
              <div className="col-md-3">
                <label className="form-label">Serial *</label>
                <input className="form-control" required maxLength={50}
                  value={form.serial} onChange={(e) => setForm({ ...form, serial: e.target.value })} />
              </div>
              <div className="col-md-5">
                <label className="form-label">Título *</label>
                <input className="form-control" required maxLength={300}
                  value={form.titulo} onChange={(e) => setForm({ ...form, titulo: e.target.value })} />
              </div>
              <div className="col-md-2">
                <label className="form-label">Año Estreno *</label>
                <input type="number" className="form-control" required min={1888} max={2100}
                  value={form.anoEstreno} onChange={(e) => setForm({ ...form, anoEstreno: e.target.value })} />
              </div>
              <div className="col-md-2">
                <label className="form-label">Tipo *</label>
                <select className="form-select" required value={form.tipoId}
                  onChange={(e) => setForm({ ...form, tipoId: e.target.value })}>
                  <option value="">Seleccionar...</option>
                  {tipos.map((t) => <option key={t.id} value={t.id}>{t.nombre}</option>)}
                </select>
              </div>
              <div className="col-md-4">
                <label className="form-label">URL *</label>
                <input className="form-control" required maxLength={500}
                  value={form.url} onChange={(e) => setForm({ ...form, url: e.target.value })} />
              </div>
              <div className="col-md-4">
                <label className="form-label">Imagen Portada</label>
                <input className="form-control" maxLength={500}
                  value={form.imagenPortada} onChange={(e) => setForm({ ...form, imagenPortada: e.target.value })} />
              </div>
              <div className="col-md-4">
                <label className="form-label">Sinopsis</label>
                <input className="form-control" maxLength={2000}
                  value={form.sinopsis} onChange={(e) => setForm({ ...form, sinopsis: e.target.value })} />
              </div>
              <div className="col-md-4">
                <label className="form-label">Género *</label>
                <select className="form-select" required value={form.generoId}
                  onChange={(e) => setForm({ ...form, generoId: e.target.value })}>
                  <option value="">Seleccionar...</option>
                  {generos.map((g) => <option key={g.id} value={g.id}>{g.nombre}</option>)}
                </select>
              </div>
              <div className="col-md-4">
                <label className="form-label">Director *</label>
                <select className="form-select" required value={form.directorId}
                  onChange={(e) => setForm({ ...form, directorId: e.target.value })}>
                  <option value="">Seleccionar...</option>
                  {directores.map((d) => <option key={d.id} value={d.id}>{d.nombres}</option>)}
                </select>
              </div>
              <div className="col-md-4">
                <label className="form-label">Productora *</label>
                <select className="form-select" required value={form.productoraId}
                  onChange={(e) => setForm({ ...form, productoraId: e.target.value })}>
                  <option value="">Seleccionar...</option>
                  {productoras.map((p) => <option key={p.id} value={p.id}>{p.nombre}</option>)}
                </select>
              </div>
            </div>
            <div className="mt-3 d-flex gap-2">
              <button type="submit" className="btn btn-primary">{editing ? 'Actualizar' : 'Crear'}</button>
              {editing && <button type="button" className="btn btn-secondary" onClick={cancel}>Cancelar</button>}
            </div>
          </form>
        </div>
      </div>

      {loading ? <p>Cargando...</p> : (
        <div className="table-responsive">
          <table className="table table-striped">
            <thead>
              <tr>
                <th>ID</th><th>Serial</th><th>Título</th><th>Año</th>
                <th>Tipo</th><th>Género</th><th>Director</th><th>Productora</th><th>Acciones</th>
              </tr>
            </thead>
            <tbody>
              {items.map((item) => (
                <tr key={item.id}>
                  <td>{item.id}</td>
                  <td>{item.serial}</td>
                  <td>{item.titulo}</td>
                  <td>{item.anoEstreno}</td>
                  <td>{item.nombreTipo}</td>
                  <td>{item.nombreGenero}</td>
                  <td>{item.nombreDirector}</td>
                  <td>{item.nombreProductora}</td>
                  <td>
                    <button className="btn btn-sm btn-warning me-1" onClick={() => handleEdit(item)}>Editar</button>
                    <button className="btn btn-sm btn-danger" onClick={() => handleDelete(item.id)}>Eliminar</button>
                  </td>
                </tr>
              ))}
              {items.length === 0 && <tr><td colSpan={9} className="text-center">No hay media</td></tr>}
            </tbody>
          </table>
        </div>
      )}
    </div>
  );
}
