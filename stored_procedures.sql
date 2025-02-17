CREATE OR REPLACE FUNCTION sp_crear_rol(p_nombre_rol VARCHAR)
RETURNS INT AS $$
DECLARE
    new_id INT;
BEGIN
    INSERT INTO public.rol (nombre_rol, eliminado) 
    VALUES (p_nombre_rol, false)
    RETURNING id_rol INTO new_id;
    
    RETURN new_id;
END;
$$ LANGUAGE plpgsql;

---------------------------------------------------------------------- 

CREATE OR REPLACE FUNCTION sp_obtener_roles()
RETURNS TABLE (id_rol INT, nombre_rol VARCHAR, eliminado BOOLEAN) AS $$
BEGIN
    RETURN QUERY SELECT id_rol, nombre_rol, eliminado FROM public.rol;
END;
$$ LANGUAGE plpgsql;

---------------------------------------------------------------------- 

CREATE OR REPLACE FUNCTION sp_obtener_rol_por_id(p_id_rol INT)
RETURNS TABLE (id_rol INT, nombre_rol VARCHAR, eliminado BOOLEAN) AS $$
BEGIN
    RETURN QUERY SELECT id_rol, nombre_rol, eliminado 
    FROM public.rol 
    WHERE id_rol = p_id_rol;
END;
$$ LANGUAGE plpgsql;

---------------------------------------------------------------------- 

CREATE OR REPLACE FUNCTION sp_actualizar_rol(p_id_rol INT, p_nombre_rol VARCHAR)
RETURNS VOID AS $$
BEGIN
    UPDATE public.rol 
    SET nombre_rol = p_nombre_rol
    WHERE id_rol = p_id_rol;
END;
$$ LANGUAGE plpgsql;

---------------------------------------------------------------------- 

CREATE OR REPLACE FUNCTION sp_eliminar_rol(p_id_rol INT)
RETURNS VOID AS $$
BEGIN
    UPDATE public.rol 
    SET eliminado = true 
    WHERE id_rol = p_id_rol;
END;
$$ LANGUAGE plpgsql;
