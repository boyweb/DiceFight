using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MainCanvas : MonoBehaviour
{
    #region Instance

    public static MainCanvas Instance;

    public MainCanvas()
    {
        Instance = this;
    }

    #endregion

    #region Properties

    [SerializeField] private RectTransform mainCanvasTransform;
    public RectTransform MainCanvasTransform => mainCanvasTransform;

    [SerializeField] private Button fightButton;

    #endregion

    #region Init

    private void Start()
    {
        Init();
    }

    private void Init(bool init = true)
    {
        InitHero(init);
        InitEnemy();
    }

    private void InitHero(bool init = true)
    {
        hero.MaxHitPoint = 100;
        hero.HitPoint = hero.MaxHitPoint;
        hero.IsDead = false;

        if (init) InitHeroDice();
    }

    private void InitHeroDice()
    {
        for (var i = 0; i < hero.DiceNumber; i++)
        {
            ownerDiceContainer.AddDice();
        }
    }

    private void InitEnemy()
    {
        enemy.MaxHitPoint = Random.Range(40, 81);
        enemy.HitPoint = enemy.MaxHitPoint;
        enemy.IsDead = false;

        enemy.NextTurn();
    }

    #endregion

    #region Fight

    [SerializeField] private Hero hero;
    [SerializeField] private Enemy enemy;

    [SerializeField] private DiceContainer ownerDiceContainer;

    [SerializeField] private FightAction attackValue;
    [SerializeField] private FightAction attackTimes;
    [SerializeField] private FightAction defence;
    [SerializeField] private FightAction regeneration;

    [SerializeField] private X2Action x2Action;

    [SerializeField] private BattleText battleTextPrefab;

    public IEnumerator FightCoroutine()
    {
        yield return new WaitForSeconds(0.5f);

        var heroRBT = Instantiate(
            battleTextPrefab,
            heroHitPointBar.transform.position,
            Quaternion.identity,
            transform);
        heroRBT.Regeneration(hero.RegenerationAction());
        yield return new WaitForSeconds(0.5f);

        for (var i = 0; i < hero.AttackTimes; i++)
        {
            var enemyHBT = Instantiate(
                battleTextPrefab,
                enemyHitPointBar.transform.position,
                Quaternion.identity,
                transform);
            enemyHBT.Hurt(hero.AttackAction(enemy));
            yield return new WaitForSeconds(0.5f);
        }

        yield return new WaitForSeconds(1);

        if (enemy.IsDead)
        {
            InitEnemy();
        }
        else
        {
            var enemyRBT = Instantiate(
                battleTextPrefab,
                enemyHitPointBar.transform.position,
                Quaternion.identity,
                transform);
            enemyRBT.Regeneration(enemy.RegenerationAction());
            yield return new WaitForSeconds(0.5f);

            for (var i = 0; i < enemy.AttackTimes; i++)
            {
                var heroHBT = Instantiate(
                    battleTextPrefab,
                    heroHitPointBar.transform.position,
                    Quaternion.identity,
                    transform);
                heroHBT.Hurt(enemy.AttackAction(hero));
                yield return new WaitForSeconds(0.5f);
            }

            if (hero.IsDead)
            {
                Init(false);
            }
            else
            {
                enemy.NextTurn();
            }
        }

        Restart();
    }

    public void Fight()
    {
        fightButton.gameObject.SetActive(false);

        attackValue.Over();
        attackTimes.Over();
        defence.Over();
        regeneration.Over();

        hero.AttackValue = 1 + attackValue.Value;
        hero.AttackTimes = 1 + attackTimes.Value;
        hero.Defence = 1 + defence.Value;
        hero.Regeneration = 1 + regeneration.Value;

        x2Action.Over();

        StartCoroutine(FightCoroutine());
    }

    public void Restart()
    {
        fightButton.gameObject.SetActive(true);

        ownerDiceContainer.Restart();
        InitHeroDice();

        x2Action.Restart();

        attackValue.Restart();
        attackTimes.Restart();
        defence.Restart();
        regeneration.Restart();
    }

    #endregion

    #region Fight Info

    [Header("Fight Info")] [SerializeField]
    private Slider heroHitPointBar;

    [SerializeField] private Text heroHitPointText;

    [SerializeField] private Slider enemyHitPointBar;
    [SerializeField] private Text enemyHitPointText;

    [SerializeField] private Text enemyAttackValue;
    [SerializeField] private Text enemyAttackTimes;
    [SerializeField] private Text enemyDefence;
    [SerializeField] private Text enemyRegeneration;

    private void Update()
    {
        heroHitPointBar.value = (float) hero.HitPoint / hero.MaxHitPoint;
        heroHitPointText.text = $"{hero.HitPoint}/{hero.MaxHitPoint}";

        enemyHitPointBar.value = (float) enemy.HitPoint / enemy.MaxHitPoint;
        enemyHitPointText.text = $"{enemy.HitPoint}/{enemy.MaxHitPoint}";

        enemyAttackValue.text = $"攻击：{enemy.AttackValue}";
        enemyAttackTimes.text = $"攻击次数：{enemy.AttackTimes}";
        enemyDefence.text = $"防御：{enemy.Defence}";
        enemyRegeneration.text = $"恢复：{enemy.Regeneration}";
    }

    #endregion
}