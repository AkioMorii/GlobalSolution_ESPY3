namespace GS2_Domain.Entities
{
    public class RefreshToken
    {
        public int RefreshTokenId { get; private set; }
        public int UsuarioId { get; private set; }
        public string TokenHash { get; private set; } // SHA256 do token
        public DateTime ExpiresAt { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? RevokedAt { get; private set; }
        public string? ReplacedByHash { get; private set; } // link com token substituto (hash)

        protected RefreshToken() { }

        public RefreshToken(int usuarioId, string tokenHash, DateTime expiresAt)
        {
            UsuarioId = usuarioId;
            TokenHash = tokenHash ?? throw new ArgumentNullException(nameof(tokenHash));
            ExpiresAt = expiresAt;
            CreatedAt = DateTime.UtcNow;
        }

        public bool IsExpired() => DateTime.UtcNow >= ExpiresAt;
        public bool IsActive() => RevokedAt == null && !IsExpired();

        public void Revoke(string? replacedByHash = null)
        {
            RevokedAt = DateTime.UtcNow;
            ReplacedByHash = replacedByHash;
        }
    }
}
