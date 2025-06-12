using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using System.Net.Http.Json;
using WaveClubAppEscritorio2.Models;
using System.Windows;


namespace WaveClubAppEscritorio2.Services
{
    public class APIClient
    {
        private readonly HttpClient _httpClient;

        public APIClient()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("http://localhost:5021/");

        }

        //LOGIN
        public async Task<LoginResponse?> LoginAsync(string username, string password)
        {
            try
            {
                var loginData = new { Username = username, Password = password };
                var content = new StringContent(JsonConvert.SerializeObject(loginData), Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("api/Login", content);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var loginResponse = JsonConvert.DeserializeObject<LoginResponse>(json);
                    return loginResponse;
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }
        }

        //USERS
        public async Task<List<User>> GetUsersAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<User>>("User");
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<User>($"User/{id}");
        }

        public async Task<bool> UpdateUserAsync(User userFromForm)
        {
            try
            {
                if (string.IsNullOrEmpty(userFromForm.Password))
                {
                    var existingUser = await _httpClient.GetFromJsonAsync<User>($"User/{userFromForm.Id}");
                    userFromForm.Password = existingUser.Password;
                }

                var response = await _httpClient.PutAsJsonAsync($"User/{userFromForm.Id}", userFromForm);

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"Error al actualizar usuario.\nCódigo: {(int)response.StatusCode} - {response.ReasonPhrase}\nDetalle: {errorContent}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Excepción al actualizar usuario:\n{ex.Message}", "Excepción", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        public async Task<bool> DeleteUserAsync(int userId)
        {
            var response = await _httpClient.DeleteAsync($"User/{userId}");
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> CreateUserAsync(User user)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("User", user);

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Usuario creado correctamente", "Éxito");
                    return true;
                }
                else
                {
                    var errorDetails = await response.Content.ReadAsStringAsync();

                    if (response.StatusCode == System.Net.HttpStatusCode.BadRequest &&
                        errorDetails.Contains("ya está en uso"))
                    {
                        MessageBox.Show("Ese nombre de usuario ya está en uso. Por favor, elige otro.", "Nombre de usuario en uso");
                    }
                    else
                    {
                        MessageBox.Show($"Error al crear usuario: {response.StatusCode}\nDetalles: {errorDetails}", "Error");
                    }

                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Excepción al crear usuario: {ex.Message}", "Error");
                return false;
            }
        }

        //ACTIVITIES
        public async Task<List<Activity>> GetActivitiesAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<Activity>>("Activity");
        }

        public async Task<Activity> GetActivityByIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<Activity>($"Activity/{id}");
        }


        public async Task<Activity> CreateActivityAsync(Activity activity)
        {
            var response = await _httpClient.PostAsJsonAsync("Activity", activity);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<Activity>();
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Error al crear actividad: {response.StatusCode} - {error}");
                return null;
            }
        }

        public async Task<bool> UpdateActivityAsync(Activity activity)
        {
            var response = await _httpClient.PutAsJsonAsync($"Activity/{activity.Id}", activity);
            return response.IsSuccessStatusCode;
        }


        public async Task<bool> DeleteActivityAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"Activity/{id}");
            return response.IsSuccessStatusCode;
        }

        //BONDS

        public async Task<List<Bond>> GetBondsAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<Bond>>("Bond");
        }

        public async Task<Bond> GetBondByIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<Bond>($"Bond/{id}");
        }

        public async Task<Bond?> CreateBondAsync(Bond bond)
        {
            var response = await _httpClient.PostAsJsonAsync("Bond", bond);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<Bond>();
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Error al crear bono: {response.StatusCode} - {error}");
                return null;
            }
        }

        public async Task<bool> UpdateBondAsync(Bond bond)
        {
            var response = await _httpClient.PutAsJsonAsync($"Bond/{bond.Id}", bond);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteBondAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"Bond/{id}");
            return response.IsSuccessStatusCode;
        }

        //BOOKING
        public async Task<List<Booking>> GetBookingsAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<Booking>>("Booking");
        }

        public async Task<Booking?> GetBookingByIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<Booking>($"Booking/{id}");
        }

        public async Task<Booking?> CreateBookingAsync(Booking booking)
        {
            try
            {
                booking.Activity = await GetActivityByIdAsync(booking.IdActivity);
                booking.User = await GetUserByIdAsync(booking.IdUser);

                var response = await _httpClient.PostAsJsonAsync("Booking", booking);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<Booking>();
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Error al crear reserva: {response.StatusCode} - {error}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Excepción al crear reserva: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> UpdateBookingAsync(Booking booking)
        {
            var response = await _httpClient.PutAsJsonAsync($"Booking/{booking.Id}", booking);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteBookingAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"Booking/{id}");
            return response.IsSuccessStatusCode;
        }

        //USERBONDS
        public async Task<List<UserBond>> GetUserBondsAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<UserBond>>("UserBond");
        }

        public async Task<UserBond?> CreateUserBondAsync(int userId, int bondId, int remainingClasses)
        {
            try
            {
                var request = new AcquireBondRequest
                {
                    UserId = userId,
                    BondId = bondId,
                    RemainingClasses = remainingClasses
                };

                var response = await _httpClient.PostAsJsonAsync("UserBond/AcquireBond", request);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<UserBond>();
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Error al adquirir bono: {response.StatusCode} - {error}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Excepción al adquirir bono: {ex.Message}");
                return null;
            }
        }


        public async Task<bool> UpdateUserBondAsync(UserBond bond)
        {
            var response = await _httpClient.PutAsJsonAsync($"UserBond/{bond.Id}", bond);
            return response.IsSuccessStatusCode;
        }


        public async Task<bool> DeleteUserBondAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"UserBond/{id}");
            return response.IsSuccessStatusCode;
        }


        //EMPLOYEE
        public async Task<List<Employee>> GetEmployeesAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<Employee>>("Employee");
        }

        public async Task<Employee> GetEmployeeByIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<Employee>($"Employee/{id}");
        }

        public async Task<Employee> CreateEmployeeAsync(Employee employee)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("Employee", employee);

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<Employee>();
                }
                else
                {
                    var errorDetails = await response.Content.ReadAsStringAsync();

                    if (response.StatusCode == System.Net.HttpStatusCode.BadRequest &&
                        errorDetails.Contains("ya está en uso"))
                    {
                        MessageBox.Show("Ese nombre de usuario ya está en uso. Por favor, elige otro.", "Nombre de usuario en uso");
                    }
                    else
                    {
                        MessageBox.Show($"Error al crear empleado: {response.StatusCode}\nDetalles: {errorDetails}", "Error");
                    }

                    return null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Excepción al crear empleado: {ex.Message}", "Error");
                return null;
            }
        }

        public async Task<bool> UpdateEmployeeAsync(Employee employee)
        {
            var response = await _httpClient.PutAsJsonAsync($"Employee/{employee.Id}", employee);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteEmployeeAsync(int employeeId)
        {
            var response = await _httpClient.DeleteAsync($"Employee/{employeeId}");
            return response.IsSuccessStatusCode;
        }

        //PARTNER
        public async Task<List<Partner>> GetPartnersAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<Partner>>("Partner");
        }

        public async Task<Partner> GetPartnerByIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<Partner>($"Partner/{id}");
        }

        public async Task<Partner> CreatePartnerAsync(Partner partner)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("Partner", partner);

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<Partner>();
                }
                else
                {
                    var errorDetails = await response.Content.ReadAsStringAsync();

                    if (response.StatusCode == System.Net.HttpStatusCode.BadRequest &&
                        errorDetails.Contains("ya está en uso"))
                    {
                        MessageBox.Show("Ese nombre de usuario ya está en uso. Por favor, elige otro.", "Nombre de usuario en uso");
                    }
                    else
                    {
                        MessageBox.Show($"Error al crear partner: {response.StatusCode}\nDetalles: {errorDetails}", "Error");
                    }

                    return null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Excepción al crear partner: {ex.Message}", "Error");
                return null;
            }
        }

        public async Task<bool> UpdatePartnerAsync(Partner partner)
        {
            var response = await _httpClient.PutAsJsonAsync($"Partner/{partner.Id}", partner);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeletePartnerAsync(int partnerId)
        {
            var response = await _httpClient.DeleteAsync($"Partner/{partnerId}");
            return response.IsSuccessStatusCode;
        }

    }
}