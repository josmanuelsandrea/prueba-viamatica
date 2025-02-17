--
-- PostgreSQL database dump
--

-- Dumped from database version 17.2
-- Dumped by pg_dump version 17.2

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET transaction_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

--
-- Name: sp_actualizar_rol(integer, character varying); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public.sp_actualizar_rol(p_id_rol integer, p_nombre_rol character varying) RETURNS void
    LANGUAGE plpgsql
    AS $$
BEGIN
    UPDATE public.rol 
    SET nombre_rol = p_nombre_rol
    WHERE id_rol = p_id_rol;
END;
$$;


ALTER FUNCTION public.sp_actualizar_rol(p_id_rol integer, p_nombre_rol character varying) OWNER TO postgres;

--
-- Name: sp_crear_rol(character varying); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public.sp_crear_rol(p_nombre_rol character varying) RETURNS integer
    LANGUAGE plpgsql
    AS $$
DECLARE
    new_id INT;
BEGIN
    INSERT INTO public.rol (nombre_rol, eliminado) 
    VALUES (p_nombre_rol, false)
    RETURNING id_rol INTO new_id;
    
    RETURN new_id;
END;
$$;


ALTER FUNCTION public.sp_crear_rol(p_nombre_rol character varying) OWNER TO postgres;

--
-- Name: sp_eliminar_rol(integer); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public.sp_eliminar_rol(p_id_rol integer) RETURNS void
    LANGUAGE plpgsql
    AS $$
BEGIN
    UPDATE public.rol 
    SET eliminado = true 
    WHERE id_rol = p_id_rol;
END;
$$;


ALTER FUNCTION public.sp_eliminar_rol(p_id_rol integer) OWNER TO postgres;

--
-- Name: sp_obtener_rol_por_id(integer); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public.sp_obtener_rol_por_id(p_id_rol integer) RETURNS TABLE(id_rol integer, nombre_rol character varying, eliminado boolean)
    LANGUAGE plpgsql
    AS $$
BEGIN
    RETURN QUERY SELECT id_rol, nombre_rol, eliminado 
    FROM public.rol 
    WHERE id_rol = p_id_rol;
END;
$$;


ALTER FUNCTION public.sp_obtener_rol_por_id(p_id_rol integer) OWNER TO postgres;

--
-- Name: sp_obtener_roles(); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public.sp_obtener_roles() RETURNS TABLE(id_rol integer, nombre_rol character varying, eliminado boolean)
    LANGUAGE plpgsql
    AS $$
BEGIN
    RETURN QUERY SELECT id_rol, nombre_rol, eliminado FROM public.rol;
END;
$$;


ALTER FUNCTION public.sp_obtener_roles() OWNER TO postgres;

SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- Name: historial_sesiones; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.historial_sesiones (
    id_historial integer NOT NULL,
    id_usuario integer NOT NULL,
    fecha_inicio timestamp with time zone DEFAULT CURRENT_TIMESTAMP NOT NULL,
    fecha_cierre timestamp with time zone,
    exito boolean NOT NULL,
    token character varying(255),
    eliminado boolean DEFAULT false NOT NULL
);


ALTER TABLE public.historial_sesiones OWNER TO postgres;

--
-- Name: historial_sesiones_id_historial_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.historial_sesiones_id_historial_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.historial_sesiones_id_historial_seq OWNER TO postgres;

--
-- Name: historial_sesiones_id_historial_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.historial_sesiones_id_historial_seq OWNED BY public.historial_sesiones.id_historial;


--
-- Name: opciones; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.opciones (
    id_opcion integer NOT NULL,
    nombre_opcion character varying(50) NOT NULL,
    eliminado boolean DEFAULT false NOT NULL,
    url character varying(255) DEFAULT '/'::character varying NOT NULL
);


ALTER TABLE public.opciones OWNER TO postgres;

--
-- Name: opciones_id_opcion_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.opciones_id_opcion_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.opciones_id_opcion_seq OWNER TO postgres;

--
-- Name: opciones_id_opcion_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.opciones_id_opcion_seq OWNED BY public.opciones.id_opcion;


--
-- Name: persona; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.persona (
    id_persona integer NOT NULL,
    nombres character varying(80) NOT NULL,
    apellidos character varying(80) NOT NULL,
    identificacion character varying(10) NOT NULL,
    fecha_nacimiento date NOT NULL,
    eliminado boolean DEFAULT false NOT NULL
);


ALTER TABLE public.persona OWNER TO postgres;

--
-- Name: persona_id_persona_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.persona_id_persona_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.persona_id_persona_seq OWNER TO postgres;

--
-- Name: persona_id_persona_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.persona_id_persona_seq OWNED BY public.persona.id_persona;


--
-- Name: rol; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.rol (
    id_rol integer NOT NULL,
    nombre_rol character varying(50) NOT NULL,
    eliminado boolean DEFAULT false NOT NULL
);


ALTER TABLE public.rol OWNER TO postgres;

--
-- Name: rol_id_rol_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.rol_id_rol_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.rol_id_rol_seq OWNER TO postgres;

--
-- Name: rol_id_rol_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.rol_id_rol_seq OWNED BY public.rol.id_rol;


--
-- Name: rol_opciones; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.rol_opciones (
    id_rol integer NOT NULL,
    id_opcion integer NOT NULL,
    eliminado boolean DEFAULT false NOT NULL
);


ALTER TABLE public.rol_opciones OWNER TO postgres;

--
-- Name: rol_usuarios; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.rol_usuarios (
    id_rol integer NOT NULL,
    id_usuario integer NOT NULL,
    eliminado boolean DEFAULT false NOT NULL
);


ALTER TABLE public.rol_usuarios OWNER TO postgres;

--
-- Name: sesiones_activas; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.sesiones_activas (
    id_sesion integer NOT NULL,
    id_usuario integer NOT NULL,
    token character varying(255) NOT NULL,
    fecha_expiracion timestamp with time zone NOT NULL,
    fecha_inicio timestamp without time zone DEFAULT CURRENT_TIMESTAMP NOT NULL,
    eliminado boolean DEFAULT false NOT NULL
);


ALTER TABLE public.sesiones_activas OWNER TO postgres;

--
-- Name: sesiones_activas_id_sesion_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.sesiones_activas_id_sesion_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.sesiones_activas_id_sesion_seq OWNER TO postgres;

--
-- Name: sesiones_activas_id_sesion_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.sesiones_activas_id_sesion_seq OWNED BY public.sesiones_activas.id_sesion;


--
-- Name: usuarios; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.usuarios (
    id_usuario integer NOT NULL,
    intentos_inicio_sesion integer DEFAULT 0,
    username character varying(50) NOT NULL,
    password text NOT NULL,
    email character varying(120) NOT NULL,
    session_active character(1) NOT NULL,
    id_persona integer NOT NULL,
    status character varying(20) NOT NULL,
    eliminado boolean DEFAULT false NOT NULL
);


ALTER TABLE public.usuarios OWNER TO postgres;

--
-- Name: usuarios_id_usuario_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.usuarios_id_usuario_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.usuarios_id_usuario_seq OWNER TO postgres;

--
-- Name: usuarios_id_usuario_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.usuarios_id_usuario_seq OWNED BY public.usuarios.id_usuario;


--
-- Name: historial_sesiones id_historial; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.historial_sesiones ALTER COLUMN id_historial SET DEFAULT nextval('public.historial_sesiones_id_historial_seq'::regclass);


--
-- Name: opciones id_opcion; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.opciones ALTER COLUMN id_opcion SET DEFAULT nextval('public.opciones_id_opcion_seq'::regclass);


--
-- Name: persona id_persona; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.persona ALTER COLUMN id_persona SET DEFAULT nextval('public.persona_id_persona_seq'::regclass);


--
-- Name: rol id_rol; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.rol ALTER COLUMN id_rol SET DEFAULT nextval('public.rol_id_rol_seq'::regclass);


--
-- Name: sesiones_activas id_sesion; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.sesiones_activas ALTER COLUMN id_sesion SET DEFAULT nextval('public.sesiones_activas_id_sesion_seq'::regclass);


--
-- Name: usuarios id_usuario; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.usuarios ALTER COLUMN id_usuario SET DEFAULT nextval('public.usuarios_id_usuario_seq'::regclass);


--
-- Name: historial_sesiones historial_sesiones_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.historial_sesiones
    ADD CONSTRAINT historial_sesiones_pkey PRIMARY KEY (id_historial);


--
-- Name: opciones opciones_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.opciones
    ADD CONSTRAINT opciones_pkey PRIMARY KEY (id_opcion);


--
-- Name: persona persona_identificacion_key; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.persona
    ADD CONSTRAINT persona_identificacion_key UNIQUE (identificacion);


--
-- Name: persona persona_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.persona
    ADD CONSTRAINT persona_pkey PRIMARY KEY (id_persona);


--
-- Name: rol rol_nombre_rol_key; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.rol
    ADD CONSTRAINT rol_nombre_rol_key UNIQUE (nombre_rol);


--
-- Name: rol_opciones rol_opciones_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.rol_opciones
    ADD CONSTRAINT rol_opciones_pkey PRIMARY KEY (id_rol, id_opcion);


--
-- Name: rol rol_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.rol
    ADD CONSTRAINT rol_pkey PRIMARY KEY (id_rol);


--
-- Name: rol_usuarios rol_usuarios_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.rol_usuarios
    ADD CONSTRAINT rol_usuarios_pkey PRIMARY KEY (id_rol, id_usuario);


--
-- Name: sesiones_activas sesiones_activas_id_usuario_key; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.sesiones_activas
    ADD CONSTRAINT sesiones_activas_id_usuario_key UNIQUE (id_usuario);


--
-- Name: sesiones_activas sesiones_activas_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.sesiones_activas
    ADD CONSTRAINT sesiones_activas_pkey PRIMARY KEY (id_sesion);


--
-- Name: usuarios usuarios_email_key; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.usuarios
    ADD CONSTRAINT usuarios_email_key UNIQUE (email);


--
-- Name: usuarios usuarios_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.usuarios
    ADD CONSTRAINT usuarios_pkey PRIMARY KEY (id_usuario);


--
-- Name: usuarios usuarios_username_key; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.usuarios
    ADD CONSTRAINT usuarios_username_key UNIQUE (username);


--
-- Name: historial_sesiones fk_historial_sesiones_usuario; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.historial_sesiones
    ADD CONSTRAINT fk_historial_sesiones_usuario FOREIGN KEY (id_usuario) REFERENCES public.usuarios(id_usuario) ON DELETE CASCADE;


--
-- Name: rol_opciones fk_rol_opcione_opcion; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.rol_opciones
    ADD CONSTRAINT fk_rol_opcione_opcion FOREIGN KEY (id_opcion) REFERENCES public.opciones(id_opcion) ON DELETE CASCADE;


--
-- Name: rol_opciones fk_rol_opciones_rel_opcion; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.rol_opciones
    ADD CONSTRAINT fk_rol_opciones_rel_opcion FOREIGN KEY (id_opcion) REFERENCES public.opciones(id_opcion) ON DELETE CASCADE;


--
-- Name: rol_opciones fk_rol_opciones_rel_rol; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.rol_opciones
    ADD CONSTRAINT fk_rol_opciones_rel_rol FOREIGN KEY (id_rol) REFERENCES public.rol(id_rol) ON DELETE CASCADE;


--
-- Name: rol_usuarios fk_rol_usuarios_rol; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.rol_usuarios
    ADD CONSTRAINT fk_rol_usuarios_rol FOREIGN KEY (id_rol) REFERENCES public.rol(id_rol) ON DELETE CASCADE;


--
-- Name: rol_usuarios fk_rol_usuarios_usuario; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.rol_usuarios
    ADD CONSTRAINT fk_rol_usuarios_usuario FOREIGN KEY (id_usuario) REFERENCES public.usuarios(id_usuario) ON DELETE CASCADE;


--
-- Name: sesiones_activas fk_sesiones_activas_usuario; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.sesiones_activas
    ADD CONSTRAINT fk_sesiones_activas_usuario FOREIGN KEY (id_usuario) REFERENCES public.usuarios(id_usuario) ON DELETE CASCADE;


--
-- Name: usuarios fk_usuarios_persona; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.usuarios
    ADD CONSTRAINT fk_usuarios_persona FOREIGN KEY (id_persona) REFERENCES public.persona(id_persona) ON DELETE CASCADE;


--
-- PostgreSQL database dump complete
--


-- CREAR USUARIOS, ROLES, OPCIONES POR DEFECTO

-- 6.1) Crear una persona y asociar un usuario "admin" a ella
WITH new_person AS (
    INSERT INTO public.persona (nombres, apellidos, identificacion, fecha_nacimiento)
         VALUES ('Admin', 'User', '9999999999', '1990-01-01')
    RETURNING id_persona
)
INSERT INTO public.usuarios (username, password, email, session_active, id_persona, status)
SELECT
    'admin',               -- username
    'admin123',            -- password (en producción, usar hash)
    'admin@example.com',   -- email
    'N',                   -- session_active
    new_person.id_persona, -- id_persona (viene del WITH)
    'ACTIVE'               -- status
FROM new_person;

-- 6.2) Crear roles "Administrador" y "Usuario"
INSERT INTO public.rol (nombre_rol)
VALUES ('Administrador'), ('Usuario');

-- 6.3) Crear opciones de menú '/dashboard' y '/admin'
INSERT INTO public.opciones (nombre_opcion, url)
VALUES ('Dashboard', '/dashboard'),
       ('Admin',     '/admin');

-- 6.4) Asignar opciones de menú a roles
-- a) "Administrador" => puede acceder tanto a "Dashboard" como a "Admin"
INSERT INTO public.rol_opciones (id_rol, id_opcion)
SELECT r.id_rol, o.id_opcion
FROM public.rol r
JOIN public.opciones o ON o.nombre_opcion IN ('Dashboard', 'Admin')
WHERE r.nombre_rol = 'Administrador';

-- b) "Usuario" => sólo "Dashboard"
INSERT INTO public.rol_opciones (id_rol, id_opcion)
SELECT r.id_rol, o.id_opcion
FROM public.rol r
JOIN public.opciones o ON o.nombre_opcion = 'Dashboard'
WHERE r.nombre_rol = 'Usuario';

-- 6.5) Asignar el usuario "admin" al rol "Administrador"
INSERT INTO public.rol_usuarios (id_rol, id_usuario)
SELECT r.id_rol, u.id_usuario
FROM public.rol r
JOIN public.usuarios u ON u.username = 'admin'
WHERE r.nombre_rol = 'Administrador';
