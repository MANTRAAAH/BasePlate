🍕 BasePlate - Turnkey Digital Ecosystem & SaaS
===============================================

**Settore:** Food & Beverage / Hospitality (PMI, Ristoranti, B&B, Dark Kitchens, Franchising)

**Modello di Business:** White-Label B2B SaaS Multi-Tenant

**Stato Sicurezza & Compliance:** GDPR Compliant, Data Isolation by Design

BasePlate non è un semplice gestionale per la ristorazione. È il primo **Ecosistema Operativo Ibrido** progettato per risolvere contemporaneamente l'acquisizione clienti (SEO Locale) e l'efficienza in sala, abbattendo gli attriti operativi. Sviluppato da chi conosce realmente i ritmi della sala e del bancone, offre un'interfaccia zero-friction per lo staff e un motore ad alte prestazioni per la vetrina pubblica.

🚀 Architettura IT & Cloud Infrastructure (Dual-Engine)
-------------------------------------------------------

L'infrastruttura sfrutta un paradigma a microservizi logici su un database condiviso, garantendo prestazioni edge, sicurezza enterprise e costi infrastrutturali marginali vicini allo zero per ogni nuovo tenant.

### Backend & Database (Master Hub)

*   **Core API (.NET 9 / C#):** Architettura RESTful e asincrona. Gestisce l'orchestrazione, la validazione delle logiche di business e il routing dei Webhook.
    
*   **Database (PostgreSQL 16+):** Modello _Shared Schema, Multi-Tenant_. Isolamento garantito tramite filtri nativi di Entity Framework Core (IMustHaveTenant e Global Query Filters). L'ID del ristorante è estratto dinamicamente dal token JWT.
    
*   **Data Flexibility:** Utilizzo di campi JSONB per storicizzare preferenze grafiche e configurazioni specifiche dei tenant senza alterare lo schema DDL.
    
*   **Real-Time & Caching:** Hub WebSocket bidirezionale tramite **SignalR** per latenza zero tra Sala e Cucina, supportato da **Redis** (LRU cache) per servire il catalogo menu senza sovraccaricare il DB relazionale.
    

### Frontend Dual-Engine

*   **Engine 1 - Gestionale Interno (Angular + Tailwind):** SPA reattiva (SSR/CSR) per le rotte operative (/admin, /sala, /kds). Potenziata con _Service Workers_ e _IndexedDB_ per Offline Tolerance durante le micro-disconnessioni di rete nei locali.
    
*   **Engine 2 - Vetrina Pubblica (Astro + Tailwind):** Framework SSG/Hybrid déployato su infrastruttura Edge. Garantisce tempi di risposta in millisecondi (TTFB) e punteggi Lighthouse perfetti per dominare la SERP locale.
    

📦 Core Modules & Work Breakdown Structure
------------------------------------------

### 1\. SuperAdmin & Master Provisioning

*   **Multi-Tenant Orchestrator:** Pannello di telemetria e controllo globale delle risorse.
    
*   **Onboarding One-Click:** Automazione totale per nuovi clienti. Generazione tenant, iniezione dati master (IVA, categorie), provisioning SSL e webhook verso Astro per la prima build statica.
    
*   **Billing Integrato:** Collegamento nativo con Stripe Connect per gestione ricorrente degli abbonamenti SaaS.
    

### 2\. Vetrina e Conversione SEO (Astro)

*   **Edge SEO:** Pre-rendering HTML per un layout statico e velocissimo.
    
*   **Schema.org e JSON-LD Dinamico:** Iniezione profonda di microdati (Restaurant, Menu, MenuItem). Prodotti e recensioni diventano entità scansionabili nativamente da Google.
    
*   **Smart Wishlist & Carrello Ibrido:** Acquisizione ordini via QR code posizionale (es. ?tavolo=12). Funziona come catalogo interattivo o strumento di self-ordering con validazione in sala.
    

### 3\. Operatività di Sala & KDS (Angular PWA)

Progettato per abbattere gli errori sotto stress, grazie a un'ergonomia studiata direttamente sul campo.

*   **Spatial Mapping 2D:** Mappatura visuale Drag&Drop dei tavoli.
    
*   **Ergonomia Estrema (BYOD):** Dark Mode obbligatoria per ridurre l'affaticamento visivo serale e target tattili maggiorati per evitare tap accidentali durante il servizio.
    
*   **Algoritmo Vettoriale "Split-Conto":** UI visuale a "vassoi" per trascinare singoli item sui pagatori, risolvendo automaticamente divisioni complesse al centesimo.
    
*   **KDS (Kitchen Display System):** Monitor rugged suddivisi per partita (Forno, Bar, Cucina) connessi via SignalR, con timer cromatici di priorità.
    

### 4\. Master Data Management (PIM)

*   **Gestore Catalogo Multidimensionale:** Database relazionale per gestire varianti, formati, impasti speciali e supplementi con pricing dinamico.
    
*   **Motore Allergeni Propagativo:** Mappatura molti-a-molti. Un flag sull'ingrediente aggiorna in tempo reale la presenza dell'allergene su tutte le pietanze correlate.
    
*   **RBAC (Role-Based Access Control):** Viste isolate in base al ruolo (Store Manager, Cuoco, Cameriere).
    

### 5\. Hardware Integration & Fiscal Local Bridge

*   **Worker Asincrono .NET 9 (Local Bridge):** Un demone installato localmente (su Mini PC o sistemi ARM) che funge da proxy tra il Cloud e l'infrastruttura di rete locale.
    
*   **Teleassistenza & Logging Centralizzato:** Gestione flotte tramite TeamViewer/RustDesk e telemetria remota via **Serilog**, permettendo interventi silenti e proattivi senza recarsi sul posto.
    
*   **Integrazione Fiscale Standard:** Utilizzo di SDK nativi (es. Custom Windows SDK, Epson ePOS) per comandare cassetti, taglierine e Registratori Telematici senza i vincoli del browser web. Supporta retry policy avanzate per connessioni di rete instabili.
    

📈 Financial Model & Go-To-Market
---------------------------------

BasePlate trasforma una spesa passiva (le commissioni del delivery) in un investimento ad alto rendimento, offrendo ai ristoratori il controllo totale sui propri flussi di cassa e sui propri dati.

*   **Setup Iniziale (Una Tantum):** Copre l'infrastruttura locale (Hardware Bridge), il data-entry del menu, il tuning SEO e la formazione.
    
*   **Licenza SaaS Premium (Ricorrente Annuale):** Garantisce hosting ad alte prestazioni, SLA del 99.9%, failover automatici e mantenimento dell'infrastruttura vetrina.
    
*   **Upselling:** Moduli di food costing, API headless per terze parti e integrazioni con aggregatori di delivery.
