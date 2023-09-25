public interface ICrudRepository<T, ID>
{
	/// <summary>
	/// Retrieves all instances from the database.
	/// </summary>
	/// <returns></returns>
	List<T> GetAll();
	/// <summary>
	/// Retrieves a particular instance from the database by its ID.
	/// </summary>
	/// <param name="id"></param>
	/// <returns></returns>
	T GetById(ID id);
    /// <summary>
    /// Inserts a new row into the database based on the paramter.
    /// </summary>
    /// <param name="obj"></param>
    T GetByName(string name);
    /// <summary>
    /// Inserts a new row into the database based on the paramter.
    /// </summary>
    /// <param name="obj"></param>
    
    //void Add(T obj);
    /// <summary>
    /// Updates an existing row based on the provided parameters.
    /// </summary>
    /// <param name="obj"></param>
    // void Update(T obj);
    /// <summary>
    /// Deteles a record by its ID.
    /// </summary>
    /// <param name="id"></param>
    // void Delete(ID id);
}