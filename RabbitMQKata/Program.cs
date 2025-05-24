Console.WriteLine("Press Ctrl+C to exit");
        
bool running = true;
Console.CancelKeyPress += (sender, e) => {
    running = false;
    e.Cancel = true; // This prevents the default handler from terminating the program
};

while (running)
{
    Thread.Sleep(100); // Prevent CPU spinning
}
        
Console.WriteLine("Gracefully shutting down...");